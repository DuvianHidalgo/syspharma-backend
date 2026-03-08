using Microsoft.AspNetCore.Mvc;
using Syspharma.Business.Services;

namespace Syspharma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly IRolService _service;

        public RolController(IRolService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var roles = await _service.ObtenerTodos();
            return Ok(roles);
        }
    }
}