using Syspharma.Data.Repositories;
using Syspharma.Domain.DTOs;
namespace Syspharma.Business.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _repo;
        public MedicoService(IMedicoRepository repo) => _repo = repo;
        public Task<List<MedicoDto>> ObtenerTodos() => _repo.ObtenerTodos();
        public Task<MedicoDto?> ObtenerPorId(int id) => _repo.ObtenerPorId(id);
        public Task<MedicoDto> Crear(MedicoCreateDto dto) => _repo.Crear(dto);
        public Task<MedicoDto> Actualizar(MedicoUpdateDto dto) => _repo.Actualizar(dto);
        public Task<bool> CambiarEstado(int id, bool estado) => _repo.CambiarEstado(id, estado);
        public Task<bool> Eliminar(int id) => _repo.Eliminar(id);
    }
}