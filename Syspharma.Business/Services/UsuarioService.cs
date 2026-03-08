using Syspharma.Data.Repositories;
using Syspharma.Domain.DTOs;

namespace Syspharma.Business.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public Task<List<UsuarioDto>> ObtenerTodos() => _repository.ObtenerTodos();
        public Task<UsuarioDto?> ObtenerPorId(int id) => _repository.ObtenerPorId(id);
        public Task<UsuarioDto> Crear(UsuarioCreateDto dto) => _repository.Crear(dto);
        public Task<UsuarioDto> Actualizar(UsuarioUpdateDto dto) => _repository.Actualizar(dto);
        public Task<UsuarioDto> CambiarEstado(int id, bool estado) => _repository.CambiarEstado(id, estado);
    }
}