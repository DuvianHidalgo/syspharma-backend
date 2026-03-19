using Syspharma.Domain.DTOs;
namespace Syspharma.Business.Services
{
    public interface IPermisoService
    {
        Task<List<PermisoDto>> ObtenerTodos();
        Task<PermisoDto?> ObtenerPorId(int id);
        Task<PermisoDto> Crear(PermisoCreateDto dto);
        Task<PermisoDto> Actualizar(PermisoUpdateDto dto);
        Task<bool> Eliminar(int id);
    }
}