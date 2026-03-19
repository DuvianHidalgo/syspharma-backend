using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
namespace Syspharma.Data.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly SyspharmaContext _context;
        public CategoriaRepository(SyspharmaContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaDto>> ObtenerTodos()
        {
            return await _context.Categorias
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Estado = c.Estado ?? true,
                    FechaCreacion = c.FechaCreacion
                })
                .ToListAsync();
        }

        public async Task<CategoriaDto?> ObtenerPorId(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return null;
            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Estado = categoria.Estado ?? true,
                FechaCreacion = categoria.FechaCreacion
            };
        }

        public async Task<CategoriaDto> Crear(CategoriaCreateDto dto)
        {
            if (await _context.Categorias.AnyAsync(c => c.Nombre == dto.Nombre))
                throw new Exception("Ya existe una categoría con ese nombre");

            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Estado = true,
                FechaCreacion = DateTime.Now
            };
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Estado = categoria.Estado ?? true,
                FechaCreacion = categoria.FechaCreacion
            };
        }

        public async Task<CategoriaDto> Actualizar(CategoriaUpdateDto dto)
        {
            var categoria = await _context.Categorias.FindAsync(dto.Id);
            if (categoria == null)
                throw new Exception("Categoría no encontrada");

            if (await _context.Categorias.AnyAsync(c => c.Nombre == dto.Nombre && c.Id != dto.Id))
                throw new Exception("Ya existe una categoría con ese nombre");

            categoria.Nombre = dto.Nombre;
            categoria.Descripcion = dto.Descripcion;
            await _context.SaveChangesAsync();
            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Estado = categoria.Estado ?? true,
                FechaCreacion = categoria.FechaCreacion
            };
        }

        public async Task<bool> CambiarEstado(int id, bool estado)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                throw new Exception("Categoría no encontrada");

            categoria.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                throw new Exception("Categoría no encontrada");

            var tieneProductos = await _context.Productos.AnyAsync(p => p.CategoriaId == id);
            if (tieneProductos)
                throw new Exception("No se puede eliminar la categoría porque tiene productos asociados");

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}