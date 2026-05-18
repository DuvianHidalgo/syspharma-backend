using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;

namespace Syspharma.Data.Repositories
{
    public interface ICompraRepository
    {
        Task<List<CompraDto>> ObtenerTodos();
        Task<CompraDto?> ObtenerPorId(int id);
        Task<CompraDto> Crear(CompraCreateDto dto);
        Task<CompraDto> Actualizar(CompraUpdateDto dto);
        Task<bool> CambiarEstado(int id, int estadoId);
        Task<bool> Eliminar(int id);
        Task<List<object>> ObtenerEstados();
    }

    public class CompraRepository : ICompraRepository
    {
        private readonly SyspharmaContext _context;
        public CompraRepository(SyspharmaContext context) => _context = context;

        private static CompraDto MapDto(Compra c) => new CompraDto
        {
            Id = c.Id,
            NumeroCompra = c.NumeroCompra,
            ProveedorId = c.ProveedorId,
            ProveedorNombre = c.Proveedor?.Nombre ?? "",
            UsuarioId = c.UsuarioId,
            UsuarioNombre = c.Usuario?.Nombre ?? "",
            EstadoId = c.EstadoId,
            EstadoNombre = c.Estado?.Nombre ?? "",
            Subtotal = c.Subtotal,
            Iva = c.Iva,
            Total = c.Total,
            Notas = c.Notas,
            Observaciones = c.Observaciones,
            FechaCompra = c.FechaCompra,
            FechaEntrega = c.FechaEntrega,
            Detalles = c.CompraDetalles.Select(d => new CompraDetalleDto
            {
                Id = d.Id,
                ProductoId = d.ProductoId,
                ProductoNombre = d.Producto?.Nombre ?? "",
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal
            }).ToList()
        };

        private string GenerarNumeroCompra() =>
            $"COM-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";

        public async Task<List<CompraDto>> ObtenerTodos()
        {
            var compras = await _context.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .Include(c => c.Estado)
                .Include(c => c.CompraDetalles).ThenInclude(d => d.Producto)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
            return compras.Select(MapDto).ToList();
        }

        public async Task<CompraDto?> ObtenerPorId(int id)
        {
            var c = await _context.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .Include(c => c.Estado)
                .Include(c => c.CompraDetalles).ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);
            return c == null ? null : MapDto(c);
        }

        public async Task<CompraDto> Crear(CompraCreateDto dto)
        {
            var estado = await _context.EstadosCompras
                .FirstOrDefaultAsync(e => e.Nombre.ToLower() == "pendiente")
                ?? throw new Exception("Estado 'Pendiente' no encontrado");

            // Calcular totales
            var subtotal = dto.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            var iva = subtotal * (dto.PorcentajeIva / 100m);
            var total = subtotal + iva;

            var compra = new Compra
            {
                NumeroCompra = GenerarNumeroCompra(),
                ProveedorId = dto.ProveedorId,
                UsuarioId = dto.UsuarioId,
                EstadoId = estado.Id,
                Subtotal = subtotal,
                Iva = iva,
                Total = total,
                Notas = dto.Notas,
                Observaciones = dto.Observaciones,
                FechaCompra = DateTime.Now,
                FechaEntrega = dto.FechaEntrega,
                // FIX: guardar los detalles correctamente
                CompraDetalles = dto.Detalles.Select(d => new CompraDetalle
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Cantidad * d.PrecioUnitario,
                }).ToList()
            };

            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();
            return await ObtenerPorId(compra.Id) ?? MapDto(compra);
        }

        // FIX: implementar Actualizar de verdad
        public async Task<CompraDto> Actualizar(CompraUpdateDto dto)
        {
            var compra = await _context.Compras
                .Include(c => c.CompraDetalles)
                .FirstOrDefaultAsync(c => c.Id == dto.Id)
                ?? throw new Exception("Compra no encontrada");

            // Actualizar campos de cabecera
            compra.ProveedorId = dto.ProveedorId;
            compra.EstadoId = dto.EstadoId;
            compra.Notas = dto.Notas;
            compra.Observaciones = dto.Observaciones;
            compra.FechaEntrega = dto.FechaEntrega;

            // Reemplazar detalles: eliminar los viejos y agregar los nuevos
            _context.CompraDetalles.RemoveRange(compra.CompraDetalles);

            var nuevosDetalles = dto.Detalles.Select(d => new CompraDetalle
            {
                CompraId = compra.Id,
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Cantidad * d.PrecioUnitario,
            }).ToList();

            compra.CompraDetalles = nuevosDetalles;

            // Recalcular totales
            compra.Subtotal = nuevosDetalles.Sum(d => d.Subtotal);
            compra.Total = compra.Subtotal + compra.Iva;

            await _context.SaveChangesAsync();
            return await ObtenerPorId(compra.Id) ?? MapDto(compra);
        }

        // FIX: implementar Eliminar de verdad
        public async Task<bool> Eliminar(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.CompraDetalles)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compra == null) return false;

            // Eliminar detalles primero para respetar FK, luego la cabecera
            _context.CompraDetalles.RemoveRange(compra.CompraDetalles);
            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CambiarEstado(int id, int estadoId)
        {
            var c = await _context.Compras.FindAsync(id);
            if (c == null) return false;
            c.EstadoId = estadoId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> ObtenerEstados() =>
            await _context.EstadosCompras
                .Select(e => (object)new { e.Id, e.Nombre })
                .ToListAsync();
    }
}