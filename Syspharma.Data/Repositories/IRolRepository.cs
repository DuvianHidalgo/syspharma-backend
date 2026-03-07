using Syspharma.Domain.DTOs;

namespace Syspharma.Data.Repositories
{
    public interface IRolRepository
    {
        Task<List<RolDto>> ObtenerTodos();
    }
}