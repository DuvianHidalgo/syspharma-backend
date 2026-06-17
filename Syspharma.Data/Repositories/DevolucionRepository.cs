<<<<<<< Updated upstream
﻿using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
=======
using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
>>>>>>> Stashed changes

namespace Syspharma.Data.Repositories
{
    public interface IDevolucionRepository
    {
        Task<List<DevolucionDto>> ObtenerTodos();
        Task<DevolucionDto?> ObtenerPorId(int id);
<<<<<<< Updated upstream
        Task<VentaDto?> ObtenerVentaParaDevolucion(int ventaId);
        Task<DevolucionDto> Crear(DevolucionCreateDto dto);
        Task<bool> Gestionar(int id, int nuevoEstado, int usuarioGestionId);
        Task<List<object>> ObtenerEstados();
=======
        Task<DevolucionDto?> ObtenerPorVentaId(int ventaId);
        Task<DevolucionDto> Crear(DevolucionCreateDto dto);
        Task<DevolucionDto> Gestionar(int id, DevolucionGestionarDto dto);
        Task<List<EstadoDevolucionDto>> ObtenerEstados();
>>>>>>> Stashed changes
    }

    public class DevolucionRepository : IDevolucionRepository
    {
        private readonly SyspharmaContext _context;
<<<<<<< Updated upstream

        public DevolucionRepository(SyspharmaContext context)
        {
            _context = context;
        }

        // --- MAPEO ---
=======
        public DevolucionRepository(SyspharmaContext context) => _context = context;

>>>>>>> Stashed changes
        private static DevolucionDto MapDto(Devolucion d) => new DevolucionDto
        {
            Id = d.Id,
            VentaId = d.VentaId,
<<<<<<< Updated upstream
            NumeroVenta = d.Venta?.NumeroVenta ?? "N/A",
            ClienteNombre = d.Venta?.ClienteNombre ?? "N/A",
            ClienteDocumento = d.Venta?.ClienteDocumento ?? "N/A",
            UsuarioId = d.UsuarioId,
            UsuarioNombre = d.Usuario?.Nombre ?? "N/A",
            EstadoId = d.EstadoId,
            EstadoNombre = d.Estado?.Nombre ?? "Pendiente",
=======
            NumeroVenta = d.Venta?.NumeroVenta,
            UsuarioId = d.UsuarioId,
            UsuarioNombre = d.Usuario?.Nombre,
            EstadoId = d.EstadoId,
            EstadoNombre = d.Estado?.Nombre,
>>>>>>> Stashed changes
            Motivo = d.Motivo,
            Observaciones = d.Observaciones,
            TotalDevolucion = d.TotalDevolucion,
            FechaDevolucion = d.FechaDevolucion,
            FechaGestion = d.FechaGestion,
<<<<<<< Updated upstream
            Detalles = d.Detalles?.Select(dd => new DetalleDevolucionDto
            {
                Id = dd.Id,
                DevolucionId = dd.DevolucionId,
                ProductoId = dd.ProductoId,
                ProductoNombre = dd.Producto?.Nombre ?? "N/A",
                DetalleVentaId = dd.DetalleVentaId,
                CantidadDevuelta = dd.CantidadDevuelta,
                PrecioUnitario = dd.PrecioUnitario,
                SubtotalDevuelto = dd.SubtotalDevuelto
            }).ToList() ?? new List<DetalleDevolucionDto>()
        };

        public async Task<List<DevolucionDto>> ObtenerTodos()
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

        public async Task<DevolucionDto?> ObtenerPorId(int id)
        {
            var d = await _context.Devoluciones
                .Include(d => d.Venta)
                .Include(d => d.Usuario)
                .Include(d => d.Estado)
                .Include(d => d.Detalles).ThenInclude(dd => dd.Producto)
                .Include(d => d.Detalles).ThenInclude(dd => dd.DetalleVenta)
                .FirstOrDefaultAsync(d => d.Id == id);

            return d == null ? null : MapDto(d);
        }

        // Carga la venta con sus productos para que el front pueda
        // mostrar qué productos se pueden devolver
        public async Task<VentaDto?> ObtenerVentaParaDevolucion(int ventaId)
        {
            var v = await _context.Ventas
                .Include(v => v.VentaDetalles).ThenInclude(d => d.Producto)
                .Include(v => v.Estado)
                .Include(v => v.MetodoPago)
                .FirstOrDefaultAsync(v => v.Id == ventaId);

            if (v == null) return null;

            return new VentaDto
            {
                Id = v.Id,
                NumeroVenta = v.NumeroVenta,
                ClienteNombre = v.ClienteNombre ?? "Consumidor Final",
                ClienteDocumento = v.ClienteDocumento ?? "N/A",
                EstadoNombre = v.Estado?.Nombre ?? "N/A",
                MetodoPagoNombre = v.MetodoPago?.Nombre ?? "N/A",
                Total = v.Total,
                Subtotal = v.Subtotal,
                FechaVenta = v.FechaVenta,
                Detalles = v.VentaDetalles?.Select(d => new VentaDetalleDto
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    ProductoNombre = d.Producto?.Nombre ?? "N/A",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList() ?? new List<VentaDetalleDto>()
            };
=======
            UsuarioGestionId = d.UsuarioGestionId,
            Detalles = d.Detalles.Select(det => new DetalleDevolucionDto
            {
                Id = det.Id,
                DetalleVentaId = det.DetalleVentaId,
                ProductoId = det.ProductoId,
                ProductoNombre = det.Producto?.Nombre,
                CantidadDevuelta = det.CantidadDevuelta,
                PrecioUnitario = det.PrecioUnitario,
                SubtotalDevuelto = det.SubtotalDevuelto
            }).ToList()
        };

        private IQueryable<Devolucion> QueryConIncludes() =>
            _context.Devoluciones
                .Include(d => d.Venta)
                .Include(d => d.Usuario)
                .Include(d => d.Estado)
                .Include(d => d.Detalles).ThenInclude(det => det.Producto);

        public async Task<List<DevolucionDto>> ObtenerTodos() =>
            (await QueryConIncludes().OrderByDescending(d => d.FechaDevolucion).ToListAsync())
            .Select(MapDto).ToList();

        public async Task<DevolucionDto?> ObtenerPorId(int id)
        {
            var d = await QueryConIncludes().FirstOrDefaultAsync(d => d.Id == id);
            return d == null ? null : MapDto(d);
        }

        public async Task<DevolucionDto?> ObtenerPorVentaId(int ventaId)
        {
            var d = await QueryConIncludes().FirstOrDefaultAsync(d => d.VentaId == ventaId);
            return d == null ? null : MapDto(d);
>>>>>>> Stashed changes
        }

        public async Task<DevolucionDto> Crear(DevolucionCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
<<<<<<< Updated upstream
                // 1. Validar que la venta exista
                var venta = await _context.Ventas
                    .Include(v => v.VentaDetalles)
                    .FirstOrDefaultAsync(v => v.Id == dto.VentaId)
                    ?? throw new Exception("La venta no existe.");

                // 2. Validar que no haya devolución aprobada previa
                var devolucionAprobada = await _context.Devoluciones
                    .AnyAsync(d => d.VentaId == dto.VentaId && d.EstadoId == 2);

                if (devolucionAprobada)
                    throw new Exception("Esta venta ya tiene una devolución aprobada.");

                // 3. Validar cantidades — ningún producto puede devolver
                //    más de lo que se vendió originalmente
                foreach (var item in dto.Detalles)
                {
                    var detalleOriginal = venta.VentaDetalles
                        .FirstOrDefault(d => d.Id == item.DetalleVentaId)
                        ?? throw new Exception($"El detalle de venta {item.DetalleVentaId} no pertenece a esta venta.");

                    if (item.CantidadDevuelta > detalleOriginal.Cantidad)
                        throw new Exception($"La cantidad a devolver del producto " +
                            $"{item.DetalleVentaId} supera la cantidad vendida.");
                }

                // 4. Crear encabezado de la devolución (estado 1 = Pendiente)
=======
                // Obtener precios de los detalles de venta
                var detalleVentaIds = dto.Detalles.Select(d => d.DetalleVentaId).ToList();
                var detallesVenta = await _context.VentaDetalles
                    .Where(d => detalleVentaIds.Contains(d.Id))
                    .ToListAsync();

                var detalles = dto.Detalles.Select(d =>
                {
                    var dventa = detallesVenta.FirstOrDefault(dv => dv.Id == d.DetalleVentaId);
                    var precio = dventa?.PrecioUnitario ?? 0;
                    return new DetalleDevolucion
                    {
                        DetalleVentaId = d.DetalleVentaId,
                        ProductoId = d.ProductoId,
                        CantidadDevuelta = d.CantidadDevuelta,
                        PrecioUnitario = precio,
                        SubtotalDevuelto = d.CantidadDevuelta * precio
                    };
                }).ToList();

                var total = detalles.Sum(d => d.SubtotalDevuelto ?? 0);

>>>>>>> Stashed changes
                var devolucion = new Devolucion
                {
                    VentaId = dto.VentaId,
                    UsuarioId = dto.UsuarioId,
<<<<<<< Updated upstream
                    EstadoId = 1,
                    Motivo = dto.Motivo,
                    Observaciones = dto.Observaciones,
                    FechaDevolucion = DateTime.Now
                };

                _context.Devoluciones.Add(devolucion);
                await _context.SaveChangesAsync();

                // 5. Insertar detalles
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

                // 6. Calcular y guardar el total de la devolución
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Gestionar(int id, int nuevoEstado, int usuarioGestionId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Validar que exista y esté Pendiente
                var devolucion = await _context.Devoluciones
                    .Include(d => d.Detalles)
                    .FirstOrDefaultAsync(d => d.Id == id && d.EstadoId == 1)
                    ?? throw new Exception("La devolución no existe o no está en estado Pendiente.");

                // 2. Validar estado destino
                if (nuevoEstado != 2 && nuevoEstado != 3)
                    throw new Exception("Estado no válido. Use 2 (Aprobada) o 3 (Rechazada).");

                // 3. Actualizar estado
                devolucion.EstadoId = nuevoEstado;
                devolucion.FechaGestion = DateTime.Now;
                devolucion.UsuarioGestionId = usuarioGestionId;

                // 4. Si se aprueba → devolver stock y ajustar venta/turno
                if (nuevoEstado == 2)
                {
                    foreach (var detalle in devolucion.Detalles)
                    {
                        var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                        if (producto != null)
                        {
                            producto.Stock += detalle.CantidadDevuelta;
                            producto.UltimaActualizacion = DateTime.Now;
                        }
                    }

                    var venta = await _context.Ventas.FindAsync(devolucion.VentaId)
                        ?? throw new Exception("No se encontró la venta asociada.");

                    venta.EstadoId = 2; // 2 = devolucion

                    var turno = await _context.Turnos.FindAsync(venta.TurnoId)
                        ?? throw new Exception("No se encontró el turno asociado a la venta.");

                    turno.TotalVentas -= devolucion.TotalDevolucion;
                    if (turno.TotalVentas < 0) turno.TotalVentas = 0;
                    turno.ResumenVentas = Math.Max(0, turno.ResumenVentas - 1);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<object>> ObtenerEstados() =>
            await _context.EstadosDevoluciones
                .Select(e => (object)new { e.Id, e.Nombre })
                .ToListAsync();
    }
}
=======
                    EstadoId = 1, // pendiente
                    Motivo = dto.Motivo,
                    Observaciones = dto.Observaciones,
                    TotalDevolucion = total,
                    FechaDevolucion = DateTime.Now,
                    Detalles = detalles
                };

                _context.Devoluciones.Add(devolucion);

                // Restaurar stock
                foreach (var d in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(d.ProductoId);
                    if (producto != null) producto.Stock += d.CantidadDevuelta;
                }

                // Cambiar estado venta a devolucion (id=2)
                var venta = await _context.Ventas.FindAsync(dto.VentaId);
                if (venta != null) venta.EstadoId = 2;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return await ObtenerPorId(devolucion.Id) ?? MapDto(devolucion);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<DevolucionDto> Gestionar(int id, DevolucionGestionarDto dto)
        {
            var devolucion = await _context.Devoluciones.FindAsync(id)
                ?? throw new Exception("Devolución no encontrada");

            devolucion.EstadoId = dto.NuevoEstado;
            devolucion.UsuarioGestionId = dto.UsuarioGestionId;
            devolucion.FechaGestion = DateTime.Now;

            await _context.SaveChangesAsync();
            return await ObtenerPorId(id) ?? MapDto(devolucion);
        }

        public async Task<List<EstadoDevolucionDto>> ObtenerEstados() =>
            await _context.EstadosDevoluciones
                .Where(e => e.Activo)
                .Select(e => new EstadoDevolucionDto { Id = e.Id, Nombre = e.Nombre, Activo = e.Activo })
                .ToListAsync();
    }
}
>>>>>>> Stashed changes
