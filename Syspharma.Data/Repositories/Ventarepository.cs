using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syspharma.Data.Repositories
{
    public interface IVentaRepository
    {
        Task<List<VentaDto>> ObtenerTodos();
        Task<VentaDto?> ObtenerPorId(int id);
        Task<VentaDto> Crear(VentaCreateDto dto);
        Task<VentaDto> Actualizar(VentaUpdateDto dto);
        Task<bool> CambiarEstado(int id, int estadoId);
        Task<bool> Eliminar(int id);
        Task<List<object>> ObtenerEstados();
    }

    public class VentaRepository : IVentaRepository
    {
        private readonly SyspharmaContext _context;

        public VentaRepository(SyspharmaContext context)
        {
            _context = context;
        }

        // --- MAPEO DE DATOS (Muestra la info en la tabla) ---
        private static VentaDto MapDto(Venta v) => new VentaDto
        {
            Id = v.Id,
            NumeroVenta = v.NumeroVenta,
            TurnoId = v.TurnoId,
            UsuarioId = v.UsuarioId,
            UsuarioNombre = v.Usuario?.Nombre ?? "N/A",
            ClienteNombre = v.ClienteNombre,
            MetodoPagoNombre = v.MetodoPago?.Nombre ?? "Efectivo",
            EstadoNombre = v.Estado?.Nombre ?? "Completada",
            Subtotal = v.Subtotal,
            Iva = v.Iva,
            Total = v.Total, // Muestra el total real guardado en la DB
            FechaVenta = v.FechaVenta,

            // Enviamos los productos mapeados
            Detalles = v.VentaDetalles?.Select(d => new VentaDetalleDto
            {
                Id = d.Id,
                ProductoNombre = d.Producto?.Nombre ?? "Producto",
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal
            }).ToList() ?? new List<VentaDetalleDto>(),

            // ENVIAMOS LOS SERVICIOS MAPEADOS (Esto permite que el modal los vea)
            Servicios = v.VentaDetallesServicios?.Select(s => new VentaDetalleServicioDto
            {
                Id = s.Id,
                ServicioNombre = s.Servicio?.Nombre ?? "Servicio Médico",
                Cantidad = s.Cantidad,
                PrecioUnitario = s.PrecioUnitario,
                Subtotal = s.Subtotal
            }).ToList() ?? new List<VentaDetalleServicioDto>()
        };

        public async Task<List<VentaDto>> ObtenerTodos()
        {
            // Cargamos todo de la base de datos primero
            var ventas = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.MetodoPago)
                .Include(v => v.Estado)
                .Include(v => v.VentaDetalles).ThenInclude(d => d.Producto)
                .Include(v => v.VentaDetallesServicios).ThenInclude(s => s.Servicio)
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();

            // Convertimos a DTO uno por uno asegurando que las listas existan
            return ventas.Select(v => new VentaDto
            {
                Id = v.Id,
                NumeroVenta = v.NumeroVenta,
                ClienteNombre = v.ClienteNombre ?? "Consumidor Final",
                MetodoPagoNombre = v.MetodoPago?.Nombre ?? "Efectivo",
                EstadoNombre = v.Estado?.Nombre ?? "Completada",
                Total = v.Total,
                Subtotal = v.Subtotal,
                FechaVenta = v.FechaVenta,
                // PASAMOS LOS PRODUCTOS (forzando listas no nulas)
                Detalles = v.VentaDetalles?.Select(d => new VentaDetalleDto
                {
                    Id = d.Id,
                    ProductoNombre = d.Producto?.Nombre ?? "Producto",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList() ?? new List<VentaDetalleDto>(),
                // PASAMOS LOS SERVICIOS (forzando listas no nulas)
                Servicios = v.VentaDetallesServicios?.Select(s => new VentaDetalleServicioDto
                {
                    Id = s.Id,
                    ServicioNombre = s.Servicio?.Nombre ?? "Servicio",
                    Cantidad = s.Cantidad,
                    PrecioUnitario = s.PrecioUnitario,
                    Subtotal = s.Subtotal
                }).ToList() ?? new List<VentaDetalleServicioDto>()
            }).ToList();
        }

        public async Task<VentaDto?> ObtenerPorId(int id)
        {
            var v = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.MetodoPago)
                .Include(v => v.Estado)
                .Include(v => v.VentaDetalles).ThenInclude(d => d.Producto)
                .Include(v => v.VentaDetallesServicios).ThenInclude(s => s.Servicio)
                .FirstOrDefaultAsync(v => v.Id == id);
            return v == null ? null : MapDto(v);
        }

        public async Task<VentaDto> Crear(VentaCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. CÁLCULO SUMANDO PRODUCTOS Y SERVICIOS (AQUÍ ESTÁ LA MAGIA)
                decimal subtotalProductos = dto.Detalles?.Sum(d => d.Cantidad * d.PrecioUnitario) ?? 0;
                decimal subtotalServicios = dto.Servicios?.Sum(s => s.Cantidad * s.PrecioUnitario) ?? 0;

                decimal subtotalFinal = subtotalProductos + subtotalServicios;
                decimal totalFinal = subtotalFinal + (subtotalFinal * (dto.PorcentajeIva / 100));

                // 2. CREAR CABECERA
                var venta = new Venta
                {
                    NumeroVenta = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TurnoId = dto.TurnoId,
                    UsuarioId = dto.UsuarioId,
                    ClienteNombre = dto.ClienteNombre ?? "Consumidor Final",
                    MetodoPagoId = dto.MetodoPagoId,
                    EstadoId = 1,
                    Subtotal = subtotalFinal,
                    Total = totalFinal, // YA TIENE EL COSTO DE LOS SERVICIOS
                    FechaVenta = DateTime.Now
                };

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                // 3. GUARDAR PRODUCTOS
                if (dto.Detalles != null)
                {
                    foreach (var d in dto.Detalles)
                    {
                        _context.VentaDetalles.Add(new VentaDetalle
                        {
                            VentaId = venta.Id,
                            ProductoId = d.ProductoId,
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario,
                            Subtotal = d.Cantidad * d.PrecioUnitario
                        });
                        var p = await _context.Productos.FindAsync(d.ProductoId);
                        if (p != null) p.Stock -= d.Cantidad;
                    }
                }

                // 4. GUARDAR SERVICIOS (CITAS MÉDICAS)
                if (dto.Servicios != null)
                {
                    foreach (var s in dto.Servicios)
                    {
                        _context.VentaDetalleServicios.Add(new VentaDetalleServicio
                        {
                            VentaId = venta.Id,
                            ServicioId = s.ServicioId,
                            Cantidad = s.Cantidad,
                            PrecioUnitario = s.PrecioUnitario,
                            Subtotal = s.Cantidad * s.PrecioUnitario
                        });
                    }
                }

                // 5. ACTUALIZAR TURNO
                var turno = await _context.Turnos.FindAsync(dto.TurnoId);
                if (turno != null)
                {
                    turno.TotalVentas += totalFinal;
                    turno.ResumenVentas += 1;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await ObtenerPorId(venta.Id) ?? MapDto(venta);
            }
            catch (Exception ex) { await transaction.RollbackAsync(); throw new Exception(ex.Message); }
        }

        public async Task<VentaDto> Actualizar(VentaUpdateDto dto) { return null; }
        public async Task<bool> CambiarEstado(int id, int estadoId) { return true; }
        public async Task<bool> Eliminar(int id) { return true; }
        public async Task<List<object>> ObtenerEstados() => await _context.EstadosVenta.Select(e => (object)new { e.Id, e.Nombre }).ToListAsync();
    }
}