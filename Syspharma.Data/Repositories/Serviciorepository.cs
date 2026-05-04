using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;

namespace Syspharma.Data.Repositories
{
    public interface IServicioRepository
    {
        Task<List<ServicioDto>> ObtenerTodos();
        Task<ServicioDto?> ObtenerPorId(int id);
        Task<ServicioDto> Crear(ServicioCreateDto dto);
        Task<ServicioDto> Actualizar(ServicioUpdateDto dto);
        Task<bool> CambiarEstado(int id, bool estado);
        Task<bool> Eliminar(int id);
    }

    public class ServicioRepository : IServicioRepository
    {
        private readonly SyspharmaContext _context;
        public ServicioRepository(SyspharmaContext context) => _context = context;

        public async Task<List<ServicioDto>> ObtenerTodos()
        {
            return await _context.Servicios
                .Select(s => new ServicioDto { Id = s.Id, Nombre = s.Nombre, Precio = s.Precio, Estado = s.Estado })
                .ToListAsync();
        }

        public async Task<ServicioDto?> ObtenerPorId(int id)
        {
            var s = await _context.Servicios.FindAsync(id);
            return s == null ? null : new ServicioDto { Id = s.Id, Nombre = s.Nombre, Precio = s.Precio, Estado = s.Estado };
        }

        public async Task<ServicioDto> Crear(ServicioCreateDto dto)
        {
            var s = new Servicio { Nombre = dto.Nombre, Precio = dto.Precio, Estado = true };
            _context.Servicios.Add(s);
            await _context.SaveChangesAsync();
            return new ServicioDto { Id = s.Id, Nombre = s.Nombre, Precio = s.Precio, Estado = s.Estado };
        }

        public async Task<ServicioDto> Actualizar(ServicioUpdateDto dto)
        {
            var s = await _context.Servicios.FindAsync(dto.Id);
            if (s == null) throw new Exception("Servicio no encontrado");
            s.Nombre = dto.Nombre;
            s.Precio = dto.Precio;
            s.Estado = dto.Estado;
            await _context.SaveChangesAsync();
            return new ServicioDto { Id = s.Id, Nombre = s.Nombre, Precio = s.Precio, Estado = s.Estado };
        }

        public async Task<bool> CambiarEstado(int id, bool estado)
        {
            var s = await _context.Servicios.FindAsync(id);
            if (s == null) return false;
            s.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var s = await _context.Servicios.FindAsync(id);
            if (s == null) return false;
            _context.Servicios.Remove(s);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}