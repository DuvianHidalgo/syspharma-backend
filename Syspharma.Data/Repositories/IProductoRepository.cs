using Syspharma.Domain.DTOs;
namespace Syspharma.Data.Repositories
{
    public interface IProductoRepository
    {
        Task<List<ProductoDto>> ObtenerTodos();
        Task<ProductoDto?> ObtenerPorId(int id);
        Task<ProductoDto> Crear(ProductoCreateDto dto);
        Task<ProductoDto> Actualizar(ProductoUpdateDto dto);
        Task<bool> CambiarEstado(int id, bool estado);
        Task<bool> Eliminar(int id);
    }
}