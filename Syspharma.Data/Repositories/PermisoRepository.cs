using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
namespace Syspharma.Data.Repositories
{
    public class PermisoRepository : IPermisoRepository
    {
        private readonly SyspharmaContext _context;
        public PermisoRepository(SyspharmaContext context) => _context = context;

        private static PermisoDto MapDto(Permiso p) => new PermisoDto
        {
            Id = p.Id,
            Codigo = p.Codigo,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            Categoria = p.Categoria,
            FechaCreacion = p.FechaCreacion
        };

        public async Task<List<PermisoDto>> ObtenerTodos() =>
            (await _context.Permisos.ToListAsync()).Select(MapDto).ToList();

        public async Task<PermisoDto?> ObtenerPorId(int id)
        {
            var p = await _context.Permisos.FindAsync(id);
            return p == null ? null : MapDto(p);
        }

        public async Task<PermisoDto> Crear(PermisoCreateDto dto)
        {
            if (await _context.Permisos.AnyAsync(p => p.Codigo == dto.Codigo))
                throw new Exception("Ya existe un permiso con ese código");
            var p = new Permiso
            {
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Categoria = dto.Categoria,
                FechaCreacion = DateTime.Now
            };
            _context.Permisos.Add(p);
            await _context.SaveChangesAsync();
            return MapDto(p);
        }

        public async Task<PermisoDto> Actualizar(PermisoUpdateDto dto)
        {
            var p = await _context.Permisos.FindAsync(dto.Id);
            if (p == null) throw new Exception("Permiso no encontrado");
            p.Codigo = dto.Codigo;
            p.Nombre = dto.Nombre;
            p.Descripcion = dto.Descripcion;
            p.Categoria = dto.Categoria;
            await _context.SaveChangesAsync();
            return MapDto(p);
        }

        public async Task<bool> Eliminar(int id)
        {
            var p = await _context.Permisos.FindAsync(id);
            if (p == null) throw new Exception("Permiso no encontrado");
            _context.Permisos.Remove(p);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}