<<<<<<< Updated upstream
using Syspharma.Domain.DTOs;
using Syspharma.Data.Repositories;

namespace Syspharma.Business.Services;

public interface IDisponibilidadService
{
    Task GuardarHorario(int medicoId, List<HorarioDiaDto> horarios);
    Task<List<HorarioDiaDto>> ObtenerHorario(int medicoId);
    Task<List<string>> ObtenerSlots(int medicoId, string fecha);
    Task<List<DiaNoDisponibleDto>> ObtenerDiasNoDisponibles(int medicoId);
    Task<DiaNoDisponibleDto> AgregarDiaNoDisponible(DiaNoDisponibleCreateDto dto);
    Task EliminarDiaNoDisponible(int id);
}

public class DisponibilidadService : IDisponibilidadService
{
    private readonly IDisponibilidadRepository _repo;
    public DisponibilidadService(IDisponibilidadRepository repo) => _repo = repo;

    public Task GuardarHorario(int medicoId, List<HorarioDiaDto> horarios)
        => _repo.GuardarHorario(medicoId, horarios);

    public Task<List<HorarioDiaDto>> ObtenerHorario(int medicoId)
        => _repo.ObtenerHorario(medicoId);

    public Task<List<string>> ObtenerSlots(int medicoId, string fecha)
        => _repo.ObtenerSlots(medicoId, fecha);

    public Task<List<DiaNoDisponibleDto>> ObtenerDiasNoDisponibles(int medicoId)
        => _repo.ObtenerDiasNoDisponibles(medicoId);

    public Task<DiaNoDisponibleDto> AgregarDiaNoDisponible(DiaNoDisponibleCreateDto dto)
        => _repo.AgregarDiaNoDisponible(dto);

    public Task EliminarDiaNoDisponible(int id)
        => _repo.EliminarDiaNoDisponible(id);
}
=======
using Syspharma.Data.Repositories;
using Syspharma.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syspharma.Business.Services
{
    public interface IDisponibilidadService
    {
        Task<List<HorarioDto>> ObtenerHorario(int medicoId);
        Task GuardarHorario(int medicoId, List<HorarioItemDto> horarios);
        Task<List<BloqueoDto>> ObtenerBloqueos(int medicoId);
        Task<BloqueoDto> CrearBloqueo(BloqueoCreateDto dto);
        Task<bool> EliminarBloqueo(int id);
        Task<List<string>> ObtenerSlots(int medicoId, DateOnly fecha);
    }

    public class DisponibilidadService : IDisponibilidadService
    {
        private readonly IDisponibilidadRepository _repo;
        public DisponibilidadService(IDisponibilidadRepository repo) => _repo = repo;

        public Task<List<HorarioDto>> ObtenerHorario(int medicoId) => _repo.ObtenerHorario(medicoId);
        public Task GuardarHorario(int medicoId, List<HorarioItemDto> horarios) => _repo.GuardarHorario(medicoId, horarios);
        public Task<List<BloqueoDto>> ObtenerBloqueos(int medicoId) => _repo.ObtenerBloqueos(medicoId);
        public Task<BloqueoDto> CrearBloqueo(BloqueoCreateDto dto) => _repo.CrearBloqueo(dto);
        public Task<bool> EliminarBloqueo(int id) => _repo.EliminarBloqueo(id);
        public Task<List<string>> ObtenerSlots(int medicoId, DateOnly fecha) => _repo.ObtenerSlots(medicoId, fecha);
    }
}
>>>>>>> Stashed changes
