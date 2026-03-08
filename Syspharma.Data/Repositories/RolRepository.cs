using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Domain.DTOs;

namespace Syspharma.Data.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly SyspharmaContext _context;

        public RolRepository(SyspharmaContext context)
        {
            _context = context;
        }

        public async Task<List<RolDto>> ObtenerTodos()
        {
            return await _context.Roles
                .Where(r => r.Estado == true)
                .Select(r => new RolDto
                {
                    Id = r.Id,
                    Nombre = r.Nombre
                })
                .ToListAsync();
        }
    }
}