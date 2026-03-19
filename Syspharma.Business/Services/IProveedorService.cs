using Syspharma.Domain.DTOs;
namespace Syspharma.Business.Services
{
    public interface IProveedorService
    {
        Task<List<ProveedorDto>> ObtenerTodos();
        Task<ProveedorDto?> ObtenerPorId(int id);
        Task<ProveedorDto> Crear(ProveedorCreateDto dto);
        Task<ProveedorDto> Actualizar(ProveedorUpdateDto dto);
        Task<bool> CambiarEstado(int id, bool estado);
        Task<bool> Eliminar(int id);
    }
}