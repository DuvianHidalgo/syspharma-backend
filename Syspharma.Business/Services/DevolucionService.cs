using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;

namespace Syspharma.Business.Services
{
    public interface IDevolucionService
    {
        Task<List<DevolucionDto>> ObtenerTodos();
        Task<DevolucionDto?> ObtenerPorId(int id);
        Task<VentaDto?> ObtenerVentaParaDevolucion(int ventaId);
        Task<DevolucionDto> Crear(DevolucionCreateDto dto);
        Task<bool> Gestionar(int id, DevolucionGestionarDto dto);
        Task<List<EstadoDevolucionDto>> ObtenerEstados();
    }

    public class DevolucionService : IDevolucionService
    {
        private readonly SyspharmaContext _context;
        private readonly IMapper _mapper;

        public DevolucionService(SyspharmaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DevolucionDto>> ObtenerTodos()
        {
            try
            {
                var devoluciones = await _context.Devoluciones
                    .Include(d => d.Venta)
                    .Include(d => d.Usuario)
                    .Include(d => d.Estado)
                    .Include(d => d.Detalles).ThenInclude(dd => dd.Producto)
                    .Include(d => d.Detalles).ThenInclude(dd => dd.DetalleVenta)
                    .OrderByDescending(d => d.FechaDevolucion)
                    .ToListAsync();

                return devoluciones.Select(MapDto).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerTodos devoluciones: {ex.Message}");
                return new List<DevolucionDto>();
            }
        }

        public async Task<DevolucionDto?> ObtenerPorId(int id)
        {
            var devolucion = await _context.Devoluciones
                .Include(d => d.Venta)
                .Include(d => d.Usuario)
                .Include(d => d.Estado)
                .Include(d => d.Detalles).ThenInclude(dd => dd.Producto)
                .Include(d => d.Detalles).ThenInclude(dd => dd.DetalleVenta)
                .FirstOrDefaultAsync(d => d.Id == id);

            return devolucion == null ? null : MapDto(devolucion);
        }

        // Carga la venta con sus productos para que el front
        // pueda mostrar qué productos se pueden devolver
        public async Task<VentaDto?> ObtenerVentaParaDevolucion(int ventaId)
        {
            var venta = await _context.Ventas
                .Include(v => v.VentaDetalles).ThenInclude(d => d.Producto)
                .Include(v => v.Estado)
                .Include(v => v.MetodoPago)
                .FirstOrDefaultAsync(v => v.Id == ventaId);

            if (venta == null) return null;

            return new VentaDto
            {
                Id = venta.Id,
                NumeroVenta = venta.NumeroVenta,
                ClienteNombre = venta.ClienteNombre ?? "Consumidor Final",
                ClienteDocumento = venta.ClienteDocumento ?? "N/A",
                EstadoNombre = venta.Estado?.Nombre ?? "N/A",
                MetodoPagoNombre = venta.MetodoPago?.Nombre ?? "N/A",
                Total = venta.Total,
                Subtotal = venta.Subtotal,
                FechaVenta = venta.FechaVenta,
                Detalles = venta.VentaDetalles?.Select(d => new VentaDetalleDto
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    ProductoNombre = d.Producto?.Nombre ?? "N/A",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList() ?? new List<VentaDetalleDto>()
            };
        }

        public async Task<DevolucionDto> Crear(DevolucionCreateDto dto)
        {
            // 1. Validar que la venta exista
            var venta = await _context.Ventas
                .Include(v => v.VentaDetalles)
                .FirstOrDefaultAsync(v => v.Id == dto.VentaId)
                ?? throw new Exception("La venta no existe.");

            // 2. Validar que no haya devolución aprobada previa
            var tieneAprobada = await _context.Devoluciones
                .AnyAsync(d => d.VentaId == dto.VentaId && d.EstadoId == 2);

            if (tieneAprobada)
                throw new Exception("Esta venta ya tiene una devolución aprobada.");

            // 3. Validar que haya al menos un detalle
            if (dto.Detalles == null || !dto.Detalles.Any())
                throw new Exception("Debe incluir al menos un producto para devolver.");

            // 4. Validar cantidades por cada producto
            foreach (var item in dto.Detalles)
            {
                var detalleOriginal = venta.VentaDetalles
                    .FirstOrDefault(d => d.Id == item.DetalleVentaId)
                    ?? throw new Exception($"El detalle {item.DetalleVentaId} no pertenece a esta venta.");

                if (item.CantidadDevuelta <= 0)
                    throw new Exception($"La cantidad a devolver debe ser mayor a 0.");

                if (item.CantidadDevuelta > detalleOriginal.Cantidad)
                    throw new Exception($"La cantidad a devolver supera la cantidad vendida " +
                        $"(vendido: {detalleOriginal.Cantidad}, intentado: {item.CantidadDevuelta}).");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 5. Crear encabezado (estado 1 = Pendiente)
                var devolucion = new Devolucion
                {
                    VentaId = dto.VentaId,
                    UsuarioId = dto.UsuarioId,
                    EstadoId = 1,
                    Motivo = dto.Motivo,
                    Observaciones = dto.Observaciones,
                    FechaDevolucion = DateTime.Now
                };

                _context.Devoluciones.Add(devolucion);
                await _context.SaveChangesAsync();

                // 6. Insertar detalles tomando el precio del detalle original
                foreach (var item in dto.Detalles)
                {
                    var detalleOriginal = venta.VentaDetalles
                        .First(d => d.Id == item.DetalleVentaId);

                    // Calcular subtotal en backend (evitar aceptar valor enviado por el cliente)
                    var subtotalCalculado = item.CantidadDevuelta * detalleOriginal.PrecioUnitario;

                    _context.DetallesDevoluciones.Add(new DetalleDevolucion
                    {
                        DevolucionId = devolucion.Id,
                        DetalleVentaId = item.DetalleVentaId,
                        ProductoId = item.ProductoId,
                        CantidadDevuelta = item.CantidadDevuelta,
                        PrecioUnitario = detalleOriginal.PrecioUnitario,
                        SubtotalDevuelto = subtotalCalculado
                    });
                }

                await _context.SaveChangesAsync();

                // 7. Calcular y guardar el total
                devolucion.TotalDevolucion = await _context.DetallesDevoluciones
                    .Where(dd => dd.DevolucionId == devolucion.Id)
                    .SumAsync(dd => dd.SubtotalDevuelto);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await ObtenerPorId(devolucion.Id) ?? MapDto(devolucion);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                string innerMessage = ex.InnerException != null ? ex.InnerException.Message : "";
                throw new Exception($"Error al registrar la devolución: {ex.Message}. {innerMessage}");
            }
        }

        public async Task<bool> Gestionar(int id, DevolucionGestionarDto dto)
        {
            // Validar que exista y esté Pendiente (estadoId=1 en EstadosDevoluciones)
            var devolucion = await _context.Devoluciones
                .Include(d => d.Detalles)
                .FirstOrDefaultAsync(d => d.Id == id && d.EstadoId == 1)
                ?? throw new Exception("La devolución no existe o no está en estado Pendiente.");

            if (dto.NuevoEstado != 2 && dto.NuevoEstado != 3)
                throw new Exception("Estado no válido. Use 2 (Aprobada) o 3 (Rechazada).");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Actualizar estado de la devolución
                devolucion.EstadoId = dto.NuevoEstado;
                devolucion.FechaGestion = DateTime.Now;
                devolucion.UsuarioGestionId = dto.UsuarioGestionId;

                // 2. Solo si se APRUEBA (nuevoEstado=2 en EstadosDevoluciones)
                if (dto.NuevoEstado == 2)
                {
                    // 2a. Devolver stock
                    foreach (var detalle in devolucion.Detalles)
                    {
                        var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                        if (producto != null)
                        {
                            producto.Stock += detalle.CantidadDevuelta;
                            producto.UltimaActualizacion = DateTime.Now;
                        }
                    }

                    // 2b. Buscar la venta y cambiar su estado a "devolucion" (estadoId=2 en estados_venta)
                    var venta = await _context.Ventas
                        .Include(v => v.Turno)
                        .FirstOrDefaultAsync(v => v.Id == devolucion.VentaId)
                        ?? throw new Exception($"No se encontró la venta con id {devolucion.VentaId}.");

                    Console.WriteLine($"[Devolucion] Cambiando estado venta {venta.Id} de {venta.EstadoId} a 2");
                    venta.EstadoId = 2; // 2 = devolucion en estados_venta

                    // 2c. Restar del turno
                    if (venta.Turno != null)
                    {
                        Console.WriteLine($"[Devolucion] Restando {devolucion.TotalDevolucion} del turno {venta.Turno.Id}");
                        venta.Turno.TotalVentas -= devolucion.TotalDevolucion;
                        if (venta.Turno.ResumenVentas > 0)
                            venta.Turno.ResumenVentas -= 1;
                    }
                }

                // Un solo SaveChanges al final — todo o nada
                var filasAfectadas = await _context.SaveChangesAsync();
                Console.WriteLine($"[Devolucion] Filas afectadas: {filasAfectadas}");

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                string innerMessage = ex.InnerException != null ? ex.InnerException.Message : "";
                throw new Exception($"Error al gestionar la devolución: {ex.Message}. {innerMessage}");
            }
        }

        public async Task<List<EstadoDevolucionDto>> ObtenerEstados()
        {
            var estados = await _context.EstadosDevoluciones.ToListAsync();
            return estados.Select(e => new EstadoDevolucionDto
            {
                Id = e.Id,
                Nombre = e.Nombre
            }).ToList();
        }

        // --- MAPEO ---
        private static DevolucionDto MapDto(Devolucion d) => new DevolucionDto
        {
            Id = d.Id,
            VentaId = d.VentaId,
            NumeroVenta = d.Venta?.NumeroVenta ?? "N/A",
            ClienteNombre = d.Venta?.ClienteNombre ?? "N/A",
            ClienteDocumento = d.Venta?.ClienteDocumento ?? "N/A",
            UsuarioId = d.UsuarioId,
            UsuarioNombre = d.Usuario?.Nombre ?? "N/A",
            EstadoId = d.EstadoId,
            EstadoNombre = d.Estado?.Nombre ?? "Pendiente",
            Motivo = d.Motivo,
            Observaciones = d.Observaciones,
            TotalDevolucion = d.TotalDevolucion,
            FechaDevolucion = d.FechaDevolucion,
            FechaGestion = d.FechaGestion,
            Detalles = d.Detalles?.Select(dd => new DetalleDevolucionDto
            {
                Id = dd.Id,
                DevolucionId = dd.DevolucionId,
                DetalleVentaId = dd.DetalleVentaId,
                ProductoId = dd.ProductoId,
                ProductoNombre = dd.Producto?.Nombre ?? "N/A",
                CantidadDevuelta = dd.CantidadDevuelta,
                PrecioUnitario = dd.PrecioUnitario,
                SubtotalDevuelto = dd.SubtotalDevuelto
            }).ToList() ?? new List<DetalleDevolucionDto>()
        };
    }
}