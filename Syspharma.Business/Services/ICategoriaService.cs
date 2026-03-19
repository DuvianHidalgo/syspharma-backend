using Syspharma.Domain.DTOs;
namespace Syspharma.Business.Services
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> ObtenerTodos();
        Task<CategoriaDto?> ObtenerPorId(int id);
        Task<CategoriaDto> Crear(CategoriaCreateDto dto);
        Task<CategoriaDto> Actualizar(CategoriaUpdateDto dto);
        Task<bool> CambiarEstado(int id, bool estado);
        Task<bool> Eliminar(int id);
    }
}