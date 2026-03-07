using Syspharma.Data.Repositories;
using Syspharma.Domain.DTOs;

namespace Syspharma.Business.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _repository;

        public RolService(IRolRepository repository)
        {
            _repository = repository;
        }

        public Task<List<RolDto>> ObtenerTodos() => _repository.ObtenerTodos();
    }
}
