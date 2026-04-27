using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;

namespace Syspharma.Data.Repositories
{
    public interface IRolMaestroRepository
    {
        Task<List<RolDto>> ObtenerTodos();
        Task<RolDto?> ObtenerPorId(int id);
        Task<RolDto> Crear(RolCreateDto dto);
        Task<RolDto> Actualizar(RolUpdateDto dto);
        Task<bool> Eliminar(int id);

        // --- ESTOS SON LOS MÉTODOS QUE FALTAN ---
        Task<bool> CambiarEstado(int id, bool estado);
        Task<List<string>> ObtenerPermisosPorRol(int rolId);
        Task<bool> AsignarPermisos(int rolId, List<string> permisos); // Cambiado a string para coincidir con tu Service
    }

    public class RolMaestroRepository : IRolMaestroRepository
    {
        private readonly SyspharmaContext _context;
        public RolMaestroRepository(SyspharmaContext context) => _context = context;

        public async Task<List<RolDto>> ObtenerTodos()
        {
            return await _context.Roles
                .Select(r => new RolDto
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    Estado = r.Estado,
                    Permisos = _context.RolesPermisos
                        .Where(rp => rp.RoleId == r.Id)
                        .Select(rp => rp.Permiso.Codigo).ToList()
                }).ToListAsync();
        }

        public async Task<RolDto?> ObtenerPorId(int id)
        {
            var r = await _context.Roles.FindAsync(id);
            if (r == null) return null;
            return new RolDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Permisos = await _context.RolesPermisos
                    .Where(x => x.RoleId == id)
                    .Select(x => x.Permiso.Codigo).ToListAsync()
            };
        }

        public async Task<RolDto> Crear(RolCreateDto dto)
        {
            var r = new Role { Nombre = dto.Nombre, Descripcion = dto.Descripcion, Estado = true, FechaCreacion = DateTime.Now };
            _context.Roles.Add(r);
            await _context.SaveChangesAsync();
            return new RolDto { Id = r.Id, Nombre = r.Nombre };
        }

        public async Task<RolDto> Actualizar(RolUpdateDto dto)
        {
            var r = await _context.Roles.FindAsync(dto.Id);
            if (r == null) throw new Exception("Rol no encontrado");
            r.Nombre = dto.Nombre;
            r.Descripcion = dto.Descripcion;
            await _context.SaveChangesAsync();
            return new RolDto { Id = r.Id, Nombre = r.Nombre };
        }

        public async Task<bool> Eliminar(int id)
        {
            var r = await _context.Roles.Include(x => x.RolesPermisos).FirstOrDefaultAsync(x => x.Id == id);
            if (r == null) return false;
            _context.RolesPermisos.RemoveRange(r.RolesPermisos);
            _context.Roles.Remove(r);
            return await _context.SaveChangesAsync() > 0;
        }

        // --- IMPLEMENTACIÓN DE LOS MÉTODOS NUEVOS ---

        public async Task<bool> CambiarEstado(int id, bool estado)
        {
            var r = await _context.Roles.FindAsync(id);
            if (r == null) return false;
            r.Estado = estado;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<string>> ObtenerPermisosPorRol(int rolId)
        {
            return await _context.RolesPermisos
                .Where(rp => rp.RoleId == rolId)
                .Select(rp => rp.Permiso.Codigo)
                .ToListAsync();
        }

        public async Task<bool> AsignarPermisos(int rolId, List<string> permisos)
        {
            // 1. Eliminar permisos actuales
            var actuales = _context.RolesPermisos.Where(rp => rp.RoleId == rolId);
            _context.RolesPermisos.RemoveRange(actuales);

            // 2. Buscar IDs de los permisos basados en los códigos recibidos
            var permisosDb = await _context.Permisos
                .Where(p => permisos.Contains(p.Codigo))
                .Select(p => p.Id)
                .ToListAsync();

            // 3. Agregar los nuevos
            foreach (var pId in permisosDb)
            {
                _context.RolesPermisos.Add(new RolesPermiso // Verifica si es RolePermiso o RolesPermisos
                {
                    RoleId = rolId,
                    PermisoId = pId
                });
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}