using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
namespace Syspharma.Data.Repositories
{
    public class RolMaestroRepository : IRolMaestroRepository
    {
        private readonly SyspharmaContext _context;
        public RolMaestroRepository(SyspharmaContext context) => _context = context;

        private static RolDto MapDto(Role r) => new RolDto
        {
            Id = r.Id,
            Nombre = r.Nombre,
            Descripcion = r.Descripcion,
            Estado = r.Estado ?? true,
            FechaCreacion = r.FechaCreacion
        };

        public async Task<List<RolDto>> ObtenerTodos() =>
            (await _context.Roles.ToListAsync()).Select(MapDto).ToList();

        public async Task<RolDto?> ObtenerPorId(int id)
        {
            var r = await _context.Roles.FindAsync(id);
            return r == null ? null : MapDto(r);
        }

        public async Task<RolDto> Crear(RolCreateDto dto)
        {
            if (await _context.Roles.AnyAsync(r => r.Nombre == dto.Nombre))
                throw new Exception("Ya existe un rol con ese nombre");
            var r = new Role
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Estado = true,
                FechaCreacion = DateTime.Now
            };
            _context.Roles.Add(r);
            await _context.SaveChangesAsync();
            return MapDto(r);
        }

        public async Task<RolDto> Actualizar(RolUpdateDto dto)
        {
            var r = await _context.Roles.FindAsync(dto.Id);
            if (r == null) throw new Exception("Rol no encontrado");
            r.Nombre = dto.Nombre;
            r.Descripcion = dto.Descripcion;
            await _context.SaveChangesAsync();
            return MapDto(r);
        }

        public async Task<bool> CambiarEstado(int id, bool estado)
        {
            var r = await _context.Roles.FindAsync(id);
            if (r == null) throw new Exception("Rol no encontrado");
            r.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var r = await _context.Roles.FindAsync(id);
            if (r == null) throw new Exception("Rol no encontrado");

            var tieneUsuarios = await _context.Usuarios.AnyAsync(u => u.RoleId == id);
            if (tieneUsuarios)
                throw new Exception("No se puede eliminar el rol porque tiene usuarios asociados");

            _context.Roles.Remove(r);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}