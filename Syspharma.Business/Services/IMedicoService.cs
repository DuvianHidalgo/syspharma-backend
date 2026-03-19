using Syspharma.Domain.DTOs;
namespace Syspharma.Business.Services
{
    public interface IMedicoService
    {
        Task<List<MedicoDto>> ObtenerTodos();
        Task<MedicoDto?> ObtenerPorId(int id);
        Task<MedicoDto> Crear(MedicoCreateDto dto);
        Task<MedicoDto> Actualizar(MedicoUpdateDto dto);
        Task<bool> CambiarEstado(int id, bool estado);
        Task<bool> Eliminar(int id);
    }
}