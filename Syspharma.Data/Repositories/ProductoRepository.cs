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
    // --- SE AGREGA LA INTERFAZ AQUÍ ---
    public interface IProductoRepository
    {
        Task<List<ProductoDto>> ObtenerTodos();
        Task<ProductoDto?> ObtenerPorId(int id);
        Task<ProductoDto> Crear(ProductoCreateDto dto);
        Task<ProductoDto> Actualizar(ProductoUpdateDto dto);
        Task<bool> CambiarEstado(int id, bool estado);
        Task<bool> Eliminar(int id);
    }

    public class ProductoRepository : IProductoRepository
    {
        private readonly SyspharmaContext _context;

        public ProductoRepository(SyspharmaContext context)
        {
            _context = context;
        }

        private static ProductoDto MapToDto(Producto p) => new ProductoDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre,
            ProveedorId = p.ProveedorId,
            ProveedorNombre = p.Proveedor?.Nombre,
            Precio = p.Precio,
            PrecioCompra = p.PrecioCompra,
            Stock = p.Stock,
            CodigoBarras = p.CodigoBarras,
            Imagen = p.Imagen,
            Estado = p.Estado,
            FechaCreacion = p.FechaCreacion,
            UltimaActualizacion = p.UltimaActualizacion
        };

        public async Task<List<ProductoDto>> ObtenerTodos()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria.Nombre,
                    ProveedorId = p.ProveedorId,
                    ProveedorNombre = p.Proveedor != null ? p.Proveedor.Nombre : null,
                    Precio = p.Precio,
                    PrecioCompra = p.PrecioCompra,
                    Stock = p.Stock,
                    CodigoBarras = p.CodigoBarras,
                    Imagen = p.Imagen,
                    Estado = p.Estado,
                    FechaCreacion = p.FechaCreacion,
                    UltimaActualizacion = p.UltimaActualizacion
                })
                .ToListAsync();
        }

        public async Task<ProductoDto?> ObtenerPorId(int id)
        {
            var p = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            return p == null ? null : MapToDto(p);
        }

        public async Task<ProductoDto> Crear(ProductoCreateDto dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                CategoriaId = dto.CategoriaId,
                ProveedorId = dto.ProveedorId,
                Precio = dto.Precio,
                PrecioCompra = dto.PrecioCompra,
                Stock = dto.Stock ?? 0,
                CodigoBarras = dto.CodigoBarras,
                Imagen = dto.Imagen,
                Estado = true,
                FechaCreacion = DateTime.Now,
                UltimaActualizacion = DateTime.Now
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return await ObtenerPorId(producto.Id) ?? MapToDto(producto);
        }

        public async Task<ProductoDto> Actualizar(ProductoUpdateDto dto)
        {
            var producto = await _context.Productos.FindAsync(dto.Id);

            if (producto == null)
                throw new Exception("Producto no encontrado");

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.CategoriaId = dto.CategoriaId;
            producto.ProveedorId = dto.ProveedorId;
            producto.Precio = dto.Precio;
            producto.PrecioCompra = dto.PrecioCompra;
            producto.Stock = dto.Stock ?? producto.Stock;
            producto.CodigoBarras = dto.CodigoBarras;
            producto.Imagen = dto.Imagen;
            producto.UltimaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return await ObtenerPorId(producto.Id) ?? MapToDto(producto);
        }

        public async Task<bool> CambiarEstado(int id, bool estado)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            producto.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}