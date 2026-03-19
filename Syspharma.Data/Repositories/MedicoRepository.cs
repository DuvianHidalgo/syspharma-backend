using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
namespace Syspharma.Data.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly SyspharmaContext _context;
        public MedicoRepository(SyspharmaContext context) => _context = context;

        private static MedicoDto MapDto(Medico m) => new MedicoDto
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Especialidad = m.Especialidad,
            Documento = m.Documento,
            Email = m.Email,
            Telefono = m.Telefono,
            DiasLaborales = m.DiasLaborales,
            HoraInicio = m.HoraInicio?.ToString("HH:mm"),
            HoraFin = m.HoraFin?.ToString("HH:mm"),
            Intervalo = m.Intervalo,
            Estado = m.Estado ?? true,
            FechaCreacion = m.FechaCreacion
        };

        public async Task<List<MedicoDto>> ObtenerTodos() =>
            (await _context.Medicos.ToListAsync()).Select(MapDto).ToList();

        public async Task<MedicoDto?> ObtenerPorId(int id)
        {
            var m = await _context.Medicos.FindAsync(id);
            return m == null ? null : MapDto(m);
        }

        public async Task<MedicoDto> Crear(MedicoCreateDto dto)
        {
            var m = new Medico
            {
                Nombre = dto.Nombre,
                Especialidad = dto.Especialidad,
                Documento = dto.Documento,
                Email = dto.Email,
                Telefono = dto.Telefono,
                DiasLaborales = dto.DiasLaborales,
                HoraInicio = dto.HoraInicio != null ? TimeOnly.Parse(dto.HoraInicio) : null,
                HoraFin = dto.HoraFin != null ? TimeOnly.Parse(dto.HoraFin) : null,
                Intervalo = dto.Intervalo ?? 30,
                Estado = true,
                FechaCreacion = DateTime.Now
            };
            _context.Medicos.Add(m);
            await _context.SaveChangesAsync();
            return MapDto(m);
        }

        public async Task<MedicoDto> Actualizar(MedicoUpdateDto dto)
        {
            var m = await _context.Medicos.FindAsync(dto.Id);
            if (m == null) throw new Exception("Médico no encontrado");
            m.Nombre = dto.Nombre;
            m.Especialidad = dto.Especialidad;
            m.Documento = dto.Documento;
            m.Email = dto.Email;
            m.Telefono = dto.Telefono;
            m.DiasLaborales = dto.DiasLaborales;
            m.HoraInicio = dto.HoraInicio != null ? TimeOnly.Parse(dto.HoraInicio) : null;
            m.HoraFin = dto.HoraFin != null ? TimeOnly.Parse(dto.HoraFin) : null;
            m.Intervalo = dto.Intervalo;
            await _context.SaveChangesAsync();
            return MapDto(m);
        }

        public async Task<bool> CambiarEstado(int id, bool estado)
        {
            var m = await _context.Medicos.FindAsync(id);
            if (m == null) throw new Exception("Médico no encontrado");
            m.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var m = await _context.Medicos.FindAsync(id);
            if (m == null) throw new Exception("Médico no encontrado");
            _context.Medicos.Remove(m);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}