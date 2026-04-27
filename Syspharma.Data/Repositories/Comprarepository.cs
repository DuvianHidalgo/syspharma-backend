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

        private string GenerarNumeroCompra() => $"COM-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";

        public async Task<List<CompraDto>> ObtenerTodos()
        {
            var compras = await _context.Compras.Include(c => c.Proveedor).Include(c => c.Usuario).Include(c => c.Estado).Include(c => c.CompraDetalles).ThenInclude(d => d.Producto).OrderByDescending(c => c.FechaCompra).ToListAsync();
            return compras.Select(MapDto).ToList();
        }

        public async Task<CompraDto?> ObtenerPorId(int id)
        {
            var c = await _context.Compras.Include(c => c.Proveedor).Include(c => c.Usuario).Include(c => c.Estado).Include(c => c.CompraDetalles).ThenInclude(d => d.Producto).FirstOrDefaultAsync(c => c.Id == id);
            return c == null ? null : MapDto(c);
        }

        public async Task<CompraDto> Crear(CompraCreateDto dto)
        {
            var estado = await _context.EstadosCompras.FirstOrDefaultAsync(e => e.Nombre.ToLower() == "pendiente") ?? throw new Exception("Estado no encontrado");
            var compra = new Compra
            {
                NumeroCompra = GenerarNumeroCompra(),
                ProveedorId = dto.ProveedorId,
                UsuarioId = dto.UsuarioId,
                EstadoId = estado.Id,
                Subtotal = dto.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario),
                Iva = 0,
                Total = dto.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario),
                FechaCompra = DateTime.Now
            };
            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();
            return await ObtenerPorId(compra.Id) ?? MapDto(compra);
        }

        public async Task<bool> CambiarEstado(int id, int estadoId)
        {
            var c = await _context.Compras.FindAsync(id);
            if (c == null) return false;
            c.EstadoId = estadoId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CompraDto> Actualizar(CompraUpdateDto dto) { return null; }
        public async Task<bool> Eliminar(int id) { return true; }
        public async Task<List<object>> ObtenerEstados() => await _context.EstadosCompras.Select(e => (object)new { e.Id, e.Nombre }).ToListAsync();
    }
}