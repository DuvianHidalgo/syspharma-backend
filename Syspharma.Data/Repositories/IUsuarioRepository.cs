using Syspharma.Domain.DTOs;

namespace Syspharma.Data.Repositories
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioDto>> ObtenerTodos();

        Task<UsuarioDto?> ObtenerPorId(int id);
        
        Task<UsuarioDto> Crear(UsuarioCreateDto dto);

        Task<UsuarioDto> Actualizar(UsuarioUpdateDto dto);

        Task<UsuarioDto> CambiarEstado(int id, bool estado);

    }
}