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