using Syspharma.Domain.DTOs;

namespace Syspharma.Business.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDto>> ObtenerTodos();
        Task<UsuarioDto?> ObtenerPorId(int id);
        Task<UsuarioDto> Crear(UsuarioCreateDto dto);
        Task<UsuarioDto> Actualizar(UsuarioUpdateDto dto);
        Task<UsuarioDto> CambiarEstado(int id, bool estado);
    }
}
