using Syspharma.Domain.DTOs;
namespace Syspharma.Data.Repositories
{
    public interface IRolMaestroRepository
    {
        Task<List<RolDto>> ObtenerTodos();
        Task<RolDto?> ObtenerPorId(int id);
        Task<RolDto> Crear(RolCreateDto dto);
        Task<RolDto> Actualizar(RolUpdateDto dto);
        Task<bool> CambiarEstado(int id, bool estado);
        Task<bool> Eliminar(int id);
    }
}