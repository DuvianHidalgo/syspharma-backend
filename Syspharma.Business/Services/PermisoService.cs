using Syspharma.Data.Repositories;
using Syspharma.Domain.DTOs;
namespace Syspharma.Business.Services
{
    public class PermisoService : IPermisoService
    {
        private readonly IPermisoRepository _repo;
        public PermisoService(IPermisoRepository repo) => _repo = repo;
        public Task<List<PermisoDto>> ObtenerTodos() => _repo.ObtenerTodos();
        public Task<PermisoDto?> ObtenerPorId(int id) => _repo.ObtenerPorId(id);
        public Task<PermisoDto> Crear(PermisoCreateDto dto) => _repo.Crear(dto);
        public Task<PermisoDto> Actualizar(PermisoUpdateDto dto) => _repo.Actualizar(dto);
        public Task<bool> Eliminar(int id) => _repo.Eliminar(id);
    }
}