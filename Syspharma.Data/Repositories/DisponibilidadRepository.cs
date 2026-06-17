using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syspharma.Data.Repositories
{
    public interface IDisponibilidadRepository
    {
        Task<List<HorarioDto>> ObtenerHorario(int medicoId);
        Task GuardarHorario(int medicoId, List<HorarioItemDto> horarios);
        Task<List<BloqueoDto>> ObtenerBloqueos(int medicoId);
        Task<BloqueoDto> CrearBloqueo(BloqueoCreateDto dto);
        Task<bool> EliminarBloqueo(int id);
        Task<List<string>> ObtenerSlots(int medicoId, DateOnly fecha);
    }

    public class DisponibilidadRepository : IDisponibilidadRepository
    {
        private readonly SyspharmaContext _context;
        public DisponibilidadRepository(SyspharmaContext context) => _context = context;

        public async Task<List<HorarioDto>> ObtenerHorario(int medicoId) =>
            await _context.DisponibilidadHorarios
                .Where(h => h.MedicoId == medicoId)
                .Select(h => new HorarioDto
                {
                    Id = h.Id,
                    MedicoId = h.MedicoId,
                    DiaSemana = h.DiaSemana,
                    MananaInicio = h.MananaInicio,
                    MananaFin = h.MananaFin,
                    TardeInicio = h.TardeInicio,
                    TardeFin = h.TardeFin
                }).ToListAsync();

        public async Task GuardarHorario(int medicoId, List<HorarioItemDto> horarios)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existentes = await _context.DisponibilidadHorarios.Where(h => h.MedicoId == medicoId).ToListAsync();
                _context.DisponibilidadHorarios.RemoveRange(existentes);

                foreach (var h in horarios)
                {
                    _context.DisponibilidadHorarios.Add(new DisponibilidadHorario
                    {
                        MedicoId = medicoId,
                        DiaSemana = h.DiaSemana,
                        MananaInicio = h.MananaInicio,
                        MananaFin = h.MananaFin,
                        TardeInicio = h.TardeInicio,
                        TardeFin = h.TardeFin
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<BloqueoDto>> ObtenerBloqueos(int medicoId) =>
            await _context.DisponibilidadBloqueos
                .Where(b => b.MedicoId == medicoId)
                .OrderBy(b => b.FechaInicio)
                .Select(b => new BloqueoDto
                {
                    Id = b.Id,
                    MedicoId = b.MedicoId,
                    FechaInicio = b.FechaInicio,
                    FechaFin = b.FechaFin,
                    Motivo = b.Motivo
                }).ToListAsync();

        public async Task<BloqueoDto> CrearBloqueo(BloqueoCreateDto dto)
        {
            var bloqueo = new DisponibilidadBloqueo
            {
                MedicoId = dto.MedicoId,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Motivo = dto.Motivo
            };
            _context.DisponibilidadBloqueos.Add(bloqueo);
            await _context.SaveChangesAsync();
            return new BloqueoDto
            {
                Id = bloqueo.Id,
                MedicoId = bloqueo.MedicoId,
                FechaInicio = bloqueo.FechaInicio,
                FechaFin = bloqueo.FechaFin,
                Motivo = bloqueo.Motivo
            };
        }

        public async Task<bool> EliminarBloqueo(int id)
        {
            var bloqueo = await _context.DisponibilidadBloqueos.FindAsync(id);
            if (bloqueo == null) return false;
            _context.DisponibilidadBloqueos.Remove(bloqueo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> ObtenerSlots(int medicoId, DateOnly fecha)
        {
            var diaSemana = (int)fecha.DayOfWeek;
            var horario = await _context.DisponibilidadHorarios
                .FirstOrDefaultAsync(h => h.MedicoId == medicoId && h.DiaSemana == diaSemana);
            if (horario == null) return new List<string>();

            var bloqueado = await _context.DisponibilidadBloqueos
                .AnyAsync(b => b.MedicoId == medicoId && b.FechaInicio <= fecha && b.FechaFin >= fecha);
            if (bloqueado) return new List<string>();

            var slots = new List<string>();
            slots.AddRange(GenerarSlots(horario.MananaInicio, horario.MananaFin));
            slots.AddRange(GenerarSlots(horario.TardeInicio, horario.TardeFin));
            return slots;
        }

        private static List<string> GenerarSlots(string inicio, string fin)
        {
            var slots = new List<string>();
            if (!TimeSpan.TryParse(inicio, out var start) || !TimeSpan.TryParse(fin, out var end)) return slots;
            for (var t = start; t < end; t = t.Add(TimeSpan.FromMinutes(30)))
                slots.Add(t.ToString(@"hh\:mm"));
            return slots;
        }
    }
}
