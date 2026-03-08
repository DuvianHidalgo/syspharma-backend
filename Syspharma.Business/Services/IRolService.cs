using Syspharma.Domain.DTOs;

namespace Syspharma.Business.Services
{
    public interface IRolService
    {
        Task<List<RolDto>> ObtenerTodos();
    }
}