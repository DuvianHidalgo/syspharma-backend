using Syspharma.Data.Repositories;
using Syspharma.Domain.DTOs;
namespace Syspharma.Business.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;
        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }
        public Task<List<CategoriaDto>> ObtenerTodos() => _repository.ObtenerTodos();
        public Task<CategoriaDto?> ObtenerPorId(int id) => _repository.ObtenerPorId(id);
        public Task<CategoriaDto> Crear(CategoriaCreateDto dto) => _repository.Crear(dto);
        public Task<CategoriaDto> Actualizar(CategoriaUpdateDto dto) => _repository.Actualizar(dto);
        public Task<bool> CambiarEstado(int id, bool estado) => _repository.CambiarEstado(id, estado);
        public Task<bool> Eliminar(int id) => _repository.Eliminar(id);
    }
}