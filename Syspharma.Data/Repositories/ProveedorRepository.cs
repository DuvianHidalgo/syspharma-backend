using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
namespace Syspharma.Data.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly SyspharmaContext _context;
        public ProveedorRepository(SyspharmaContext context) => _context = context;

        private static ProveedorDto MapDto(Proveedore p) => new ProveedorDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Contacto = p.Contacto,
            Email = p.Email,
            Telefono = p.Telefono,
            Direccion = p.Direccion,
            TipoDocumento = p.TipoDocumento,
            Documento = p.Documento,
            Estado = p.Estado ?? true,
            FechaCreacion = p.FechaCreacion
        };

        public async Task<List<ProveedorDto>> ObtenerTodos() =>
            (await _context.Proveedores.ToListAsync()).Select(MapDto).ToList();

        public async Task<ProveedorDto?> ObtenerPorId(int id)
        {
            var p = await _context.Proveedores.FindAsync(id);
            return p == null ? null : MapDto(p);
        }

        public async Task<ProveedorDto> Crear(ProveedorCreateDto dto)
        {
            if (await _context.Proveedores.AnyAsync(p => p.Nombre == dto.Nombre))
                throw new Exception("Ya existe un proveedor con ese nombre");
            var p = new Proveedore
            {
                Nombre = dto.Nombre,
                Contacto = dto.Contacto,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion,
                TipoDocumento = dto.TipoDocumento,
                Documento = dto.Documento,
                Estado = true,
                FechaCreacion = DateTime.Now
            };
            _context.Proveedores.Add(p);
            try { await _context.SaveChangesAsync(); }
            catch (Exception ex) { throw new Exception(ex.InnerException?.Message ?? ex.Message); }
            return MapDto(p);
        }

        public async Task<ProveedorDto> Actualizar(ProveedorUpdateDto dto)
        {
            var p = await _context.Proveedores.FindAsync(dto.Id);
            if (p == null) throw new Exception("Proveedor no encontrado");
            p.Nombre = dto.Nombre;
            p.Contacto = dto.Contacto;
            p.Email = dto.Email;
            p.Telefono = dto.Telefono;
            p.Direccion = dto.Direccion;
            p.TipoDocumento = dto.TipoDocumento;
            p.Documento = dto.Documento;
            try { await _context.SaveChangesAsync(); }
            catch (Exception ex) { throw new Exception(ex.InnerException?.Message ?? ex.Message); }
            return MapDto(p);
        }

        public async Task<bool> CambiarEstado(int id, bool estado)
        {
            var p = await _context.Proveedores.FindAsync(id);
            if (p == null) throw new Exception("Proveedor no encontrado");
            p.Estado = estado;
            try { await _context.SaveChangesAsync(); }
            catch (Exception ex) { throw new Exception(ex.InnerException?.Message ?? ex.Message); }
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var p = await _context.Proveedores.FindAsync(id);
            if (p == null) throw new Exception("Proveedor no encontrado");
            var tieneProductos = await _context.Productos.AnyAsync(prod => prod.ProveedorId == id);
            if (tieneProductos)
                throw new Exception("No se puede eliminar el proveedor porque tiene productos asociados");
            _context.Proveedores.Remove(p);
            try { await _context.SaveChangesAsync(); }
            catch (Exception ex) { throw new Exception(ex.InnerException?.Message ?? ex.Message); }
            return true;
        }
    }
}