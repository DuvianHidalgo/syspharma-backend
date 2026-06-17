using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;
<<<<<<< Updated upstream

namespace Syspharma.Data.Repositories;

public interface IDisponibilidadRepository
{
    Task GuardarHorario(int medicoId, List<HorarioDiaDto> horarios);
    Task<List<HorarioDiaDto>> ObtenerHorario(int medicoId);
    Task<List<string>> ObtenerSlots(int medicoId, string fecha);
    Task<List<DiaNoDisponibleDto>> ObtenerDiasNoDisponibles(int medicoId);
    Task<DiaNoDisponibleDto> AgregarDiaNoDisponible(DiaNoDisponibleCreateDto dto);
    Task EliminarDiaNoDisponible(int id);
}

public class DisponibilidadRepository : IDisponibilidadRepository
{
    private readonly SyspharmaContext _context;
    public DisponibilidadRepository(SyspharmaContext context) => _context = context;

    public async Task GuardarHorario(int medicoId, List<HorarioDiaDto> horarios)
    {
        // Eliminar horarios actuales y reemplazar (upsert simple)
        var existentes = await _context.MedicoHorarios
            .Where(h => h.MedicoId == medicoId)
            .ToListAsync();
        _context.MedicoHorarios.RemoveRange(existentes);

        foreach (var h in horarios)
        {
            // Solo guardar días que tengan al menos un horario configurado
            if (h.MananaInicio == null && h.TardeInicio == null) continue;

            _context.MedicoHorarios.Add(new MedicoHorario
            {
                MedicoId = medicoId,
                DiaSemana = h.DiaSemana,
                MananaInicio = h.MananaInicio != null ? TimeOnly.Parse(h.MananaInicio) : null,
                MananaFin = h.MananaFin != null ? TimeOnly.Parse(h.MananaFin) : null,
                TardeInicio = h.TardeInicio != null ? TimeOnly.Parse(h.TardeInicio) : null,
                TardeFin = h.TardeFin != null ? TimeOnly.Parse(h.TardeFin) : null,
            });
        }
        await _context.SaveChangesAsync();
    }

    public async Task<List<HorarioDiaDto>> ObtenerHorario(int medicoId)
    {
        return await _context.MedicoHorarios
            .Where(h => h.MedicoId == medicoId)
            .Select(h => new HorarioDiaDto
            {
                DiaSemana = h.DiaSemana,
                MananaInicio = h.MananaInicio != null ? h.MananaInicio.Value.ToString("HH:mm") : null,
                MananaFin = h.MananaFin != null ? h.MananaFin.Value.ToString("HH:mm") : null,
                TardeInicio = h.TardeInicio != null ? h.TardeInicio.Value.ToString("HH:mm") : null,
                TardeFin = h.TardeFin != null ? h.TardeFin.Value.ToString("HH:mm") : null,
            })
            .ToListAsync();
    }

    public async Task<List<string>> ObtenerSlots(int medicoId, string fecha)
    {
        // 1. Verificar si la fecha está bloqueada
        var fechaDate = DateOnly.Parse(fecha);
        var bloqueado = await _context.MedicoDiasNoDisponibles.AnyAsync(d =>
            d.MedicoId == medicoId &&
            d.FechaInicio <= fechaDate &&
            d.FechaFin >= fechaDate);

        if (bloqueado) return new List<string>();

        // 2. Obtener horario del día de la semana
        // DateOnly.DayOfWeek: 0=Dom, 1=Lun... igual que byte DiaSemana
        var diaSemana = (byte)fechaDate.DayOfWeek;
        var horario = await _context.MedicoHorarios
            .FirstOrDefaultAsync(h => h.MedicoId == medicoId && h.DiaSemana == diaSemana);

        if (horario == null) return new List<string>();

        // 3. Obtener citas ya tomadas ese día
        var citasTomadas = await _context.Citas
            .Where(c => c.MedicoId == medicoId &&
                        c.Fecha == fechaDate &&
                        c.Estado.Nombre != "Cancelada")
            .Select(c => c.Hora)
            .ToListAsync();

        // 4. Generar slots libres
        var slots = new List<string>();
        var intervalo = 30; // minutos

        void GenerarSlots(TimeOnly? inicio, TimeOnly? fin)
        {
            if (inicio == null || fin == null) return;
            var current = inicio.Value;
            while (current < fin.Value)
            {
                if (!citasTomadas.Contains(current))
                    slots.Add(current.ToString("HH:mm"));
                current = current.AddMinutes(intervalo);
            }
        }

        GenerarSlots(horario.MananaInicio, horario.MananaFin);
        GenerarSlots(horario.TardeInicio, horario.TardeFin);

        return slots;
    }

    public async Task<List<DiaNoDisponibleDto>> ObtenerDiasNoDisponibles(int medicoId)
    {
        return await _context.MedicoDiasNoDisponibles
            .Where(d => d.MedicoId == medicoId)
            .OrderBy(d => d.FechaInicio)
            .Select(d => new DiaNoDisponibleDto
            {
                Id = d.Id,
                MedicoId = d.MedicoId,
                FechaInicio = d.FechaInicio.ToString("yyyy-MM-dd"),
                FechaFin = d.FechaFin.ToString("yyyy-MM-dd"),
                Motivo = d.Motivo
            })
            .ToListAsync();
    }

    public async Task<DiaNoDisponibleDto> AgregarDiaNoDisponible(DiaNoDisponibleCreateDto dto)
    {
        var entity = new MedicoDiaNoDisponible
        {
            MedicoId = dto.MedicoId,
            FechaInicio = DateOnly.Parse(dto.FechaInicio),
            FechaFin = DateOnly.Parse(dto.FechaFin),
            Motivo = dto.Motivo
        };
        _context.MedicoDiasNoDisponibles.Add(entity);
        await _context.SaveChangesAsync();

        return new DiaNoDisponibleDto
        {
            Id = entity.Id,
            MedicoId = entity.MedicoId,
            FechaInicio = entity.FechaInicio.ToString("yyyy-MM-dd"),
            FechaFin = entity.FechaFin.ToString("yyyy-MM-dd"),
            Motivo = entity.Motivo
        };
    }

    public async Task EliminarDiaNoDisponible(int id)
    {
        var entity = await _context.MedicoDiasNoDisponibles.FindAsync(id);
        if (entity != null)
        {
            _context.MedicoDiasNoDisponibles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
=======
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
>>>>>>> Stashed changes
