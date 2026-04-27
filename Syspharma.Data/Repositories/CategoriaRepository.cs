using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;

namespace Syspharma.Data.Repositories
{
    public interface ICategoriaRepository
    {
        Task<List<CategoriaDto>> ObtenerTodos();
        Task<CategoriaDto?> ObtenerPorId(int id);
        Task<CategoriaDto> Crear(CategoriaCreateDto dto);
        Task<CategoriaDto> Actualizar(CategoriaUpdateDto dto);
        Task<bool> Eliminar(int id);
        Task<bool> CambiarEstado(int id, bool estado); // <--- AGREGADO
    }

    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly SyspharmaContext _context;
        public CategoriaRepository(SyspharmaContext context) => _context = context;

        public async Task<List<CategoriaDto>> ObtenerTodos() {
            var lista = await _context.Categorias.ToListAsync();
            return lista.Select(c => new CategoriaDto { Id = c.Id, Nombre = c.Nombre, Descripcion = c.Descripcion, Estado = c.Estado }).ToList();
        }

        public async Task<CategoriaDto?> ObtenerPorId(int id) {
            var c = await _context.Categorias.FindAsync(id);
            return c == null ? null : new CategoriaDto { Id = c.Id, Nombre = c.Nombre, Descripcion = c.Descripcion, Estado = c.Estado };
        }

        public async Task<CategoriaDto> Crear(CategoriaCreateDto dto) {
            var c = new Categoria { Nombre = dto.Nombre, Descripcion = dto.Descripcion, Estado = true };
            _context.Categorias.Add(c);
            await _context.SaveChangesAsync();
            return new CategoriaDto { Id = c.Id, Nombre = c.Nombre, Descripcion = c.Descripcion, Estado = c.Estado };
        }

        public async Task<CategoriaDto> Actualizar(CategoriaUpdateDto dto) {
            var c = await _context.Categorias.FindAsync(dto.Id);
            if (c != null) { c.Nombre = dto.Nombre; c.Descripcion = dto.Descripcion; await _context.SaveChangesAsync(); }
            return new CategoriaDto { Id = c!.Id, Nombre = c.Nombre, Descripcion = c.Descripcion, Estado = c.Estado };
        }

        public async Task<bool> CambiarEstado(int id, bool estado) {
            var c = await _context.Categorias.FindAsync(id);
            if (c == null) return false;
            c.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id) {
            var c = await _context.Categorias.FindAsync(id);
            if (c == null) return false;
            _context.Categorias.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}