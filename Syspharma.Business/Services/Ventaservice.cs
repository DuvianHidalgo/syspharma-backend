using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;

namespace Syspharma.Business.Services
{
    public interface IVentaService
    {
        Task<List<VentaDto>> ObtenerTodos();
        Task<VentaDto?> ObtenerPorId(int id);
        Task<VentaDto> Crear(VentaCreateDto dto);
        Task<VentaDto> Actualizar(VentaUpdateDto dto);
        Task<bool> Eliminar(int id);
        Task<List<EstadoVentaDto>> ObtenerEstados();
        Task<bool> CambiarEstado(int id, int estadoId);
    }

    public class VentaService : IVentaService
    {
        private readonly SyspharmaContext _context;
        private readonly IMapper _mapper;

        public VentaService(SyspharmaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VentaDto>> ObtenerTodos()
        {
            try
            {
                var ventas = await _context.Ventas
                    .Include(v => v.Estado)
                    .Include(v => v.MetodoPago)
                    .Include(v => v.Usuario)
                    .Include(v => v.VentaDetalles).ThenInclude(d => d.Producto)
                    .Include(v => v.VentaDetallesServicios).ThenInclude(s => s.Servicio)
                    .OrderByDescending(v => v.FechaVenta)
                    .ToListAsync();

                return _mapper.Map<List<VentaDto>>(ventas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerTodos: {ex.Message}");
                return new List<VentaDto>();
            }
        }

        public async Task<VentaDto?> ObtenerPorId(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Estado)
                .Include(v => v.MetodoPago)
                .Include(v => v.Usuario)
                .Include(v => v.VentaDetalles).ThenInclude(d => d.Producto)
                .Include(v => v.VentaDetallesServicios).ThenInclude(s => s.Servicio)
                .FirstOrDefaultAsync(v => v.Id == id);

            return _mapper.Map<VentaDto>(venta);
        }

        public async Task<VentaDto> Crear(VentaCreateDto dto)
        {
            // 1. VALIDACIÓN PREVIA DE SEGURIDAD (Evita FK Conflict)
            if (dto.TurnoId <= 0)
                throw new Exception("No se puede crear la venta: El ID de Turno no es válido (0). Asegúrese de tener una caja abierta.");

            var turno = await _context.Turnos.FindAsync(dto.TurnoId);
            if (turno == null)
                throw new Exception($"El turno con ID {dto.TurnoId} no existe en la base de datos. Por favor, cierre sesión y vuelva a entrar.");

            var metodoPago = await _context.MetodosPagos.FindAsync(dto.MetodoPagoId);
            if (metodoPago == null)
                throw new Exception("El método de pago seleccionado no es válido o no existe.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 2. CÁLCULO MANUAL DE TOTALES (Para asegurar que no se guarde en $0)
                decimal subtotalProd = dto.Detalles?.Sum(d => (d.Cantidad * d.PrecioUnitario) - d.Descuento) ?? 0;
                decimal subtotalServ = dto.Servicios?.Sum(s => (s.Cantidad * s.PrecioUnitario) - s.Descuento) ?? 0;
                decimal subtotalFinal = subtotalProd + subtotalServ;

                decimal porcentajeIva = dto.PorcentajeIva > 0 ? dto.PorcentajeIva : 0;
                decimal ivaFinal = subtotalFinal * (porcentajeIva / 100);
                decimal totalFinal = subtotalFinal + ivaFinal;

                // 3. CREAR LA ENTIDAD VENTA
                var venta = new Venta
                {
                    NumeroVenta = $"VNT-{DateTime.Now:yyyyMMddHHmmss}",
                    TurnoId = dto.TurnoId,
                    UsuarioId = dto.UsuarioId,
                    ClienteNombre = string.IsNullOrWhiteSpace(dto.ClienteNombre) ? "Consumidor Final" : dto.ClienteNombre,
                    ClienteDocumento = dto.ClienteDocumento,
                    ClienteTelefono = dto.ClienteTelefono,
                    MetodoPagoId = dto.MetodoPagoId,
                    EstadoId = 1, // 1 = Completada
                    Subtotal = subtotalFinal,
                    Iva = ivaFinal,
                    PorcentajeIva = porcentajeIva,
                    Total = totalFinal,
                    Notas = dto.Notas,
                    FechaVenta = DateTime.Now,
                    Origen = string.IsNullOrWhiteSpace(dto.Origen) ? "CAJA" : dto.Origen,
                    PedidoId = dto.PedidoId
                };

                _context.Ventas.Add(venta);
                // Guardamos cambios parciales para generar el ID de la venta
                await _context.SaveChangesAsync();

                // 4. GUARDAR DETALLES DE PRODUCTOS Y ACTUALIZAR STOCK
                if (dto.Detalles != null && dto.Detalles.Any())
                {
                    foreach (var d in dto.Detalles)
                    {
                        var nuevoDetalle = new VentaDetalle
                        {
                            VentaId = venta.Id,
                            ProductoId = d.ProductoId,
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario,
                            Descuento = d.Descuento,
                            Subtotal = (d.Cantidad * d.PrecioUnitario) - d.Descuento
                        };
                        _context.VentaDetalles.Add(nuevoDetalle);

                        // Descuento de stock
                        var producto = await _context.Productos.FindAsync(d.ProductoId);
                        if (producto != null)
                        {
                            producto.Stock -= d.Cantidad;
                            producto.UltimaActualizacion = DateTime.Now;
                        }
                    }
                }

                // 5. GUARDAR DETALLES DE SERVICIOS
                if (dto.Servicios != null && dto.Servicios.Any())
                {
                    foreach (var s in dto.Servicios)
                    {
                        var nuevoServicioDetalle = new VentaDetalleServicio
                        {
                            VentaId = venta.Id,
                            ServicioId = s.ServicioId,
                            Cantidad = s.Cantidad,
                            PrecioUnitario = s.PrecioUnitario,
                            Descuento = s.Descuento,
                            Subtotal = (s.Cantidad * s.PrecioUnitario) - s.Descuento,
                            CitaId = s.CitaId
                        };
                        _context.VentaDetalleServicios.Add(nuevoServicioDetalle);

                        // Mark Cita as Pagada
                        if (s.CitaId.HasValue && s.CitaId.Value > 0)
                        {
                            var cita = await _context.Citas.FindAsync(s.CitaId.Value);
                            if (cita != null)
                            {
                                cita.VentaId = venta.Id;
                                var estadoPagada = await _context.EstadosCita.FirstOrDefaultAsync(e => e.Nombre == "Pagada");
                                if (estadoPagada != null)
                                {
                                    cita.EstadoId = estadoPagada.Id;
                                }
                            }
                        }
                    }
                }

                // 6. ACTUALIZAR TOTALES DEL TURNO (CAJA)
                turno.TotalVentas += totalFinal;
                turno.ResumenVentas += 1;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Devolvemos el DTO completo mapeado
                return await ObtenerPorId(venta.Id) ?? _mapper.Map<VentaDto>(venta);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Enviamos el detalle interno para depurar en el frontend
                string innerMessage = ex.InnerException != null ? ex.InnerException.Message : "";
                throw new Exception($"Error al procesar la venta: {ex.Message}. {innerMessage}");
            }
        }

        public async Task<VentaDto> Actualizar(VentaUpdateDto dto)
        {
            var venta = await _context.Ventas.FindAsync(dto.Id);
            if (venta == null) throw new Exception("La venta no existe.");

            _mapper.Map(dto, venta);
            await _context.SaveChangesAsync();
            return await ObtenerPorId(venta.Id) ?? _mapper.Map<VentaDto>(venta);
        }

        public async Task<bool> Eliminar(int id)
        {
            var v = await _context.Ventas.FindAsync(id);
            if (v == null) return false;
            _context.Ventas.Remove(v);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EstadoVentaDto>> ObtenerEstados()
        {
            var estados = await _context.EstadosVenta.ToListAsync();
            return _mapper.Map<List<EstadoVentaDto>>(estados);
        }

        public async Task<bool> CambiarEstado(int id, int estadoId)
        {
            var v = await _context.Ventas.FindAsync(id);
            if (v == null) return false;
            v.EstadoId = estadoId;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}