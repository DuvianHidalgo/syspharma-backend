using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Syspharma.Business.Services;
using Syspharma.Domain.DTOs;

namespace Syspharma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolMaestroController : ControllerBase
    {
        private readonly IRolMaestroService _service;
        public RolMaestroController(IRolMaestroService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.ObtenerTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.ObtenerPorId(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RolCreateDto dto) => Ok(await _service.Crear(dto));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RolUpdateDto dto) => Ok(await _service.Actualizar(dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try 
            {
                var resultado = await _service.Eliminar(id);
                if (!resultado) return NotFound(new { message = "El rol no existe" });
                
                return Ok(new { message = "Rol eliminado correctamente" });
            }
            catch (Exception ex) 
            {
                // Esto envía el mensaje de "No se puede eliminar..." al Frontend en el cuadrito rojo
                return BadRequest(new { message = ex.Message }); 
            }
        }

        [HttpGet("{id}/permisos")]
        public async Task<IActionResult> GetPermisos(int id) => Ok(await _service.ObtenerPermisos(id));

        [HttpPost("{id}/permisos")]
        public async Task<IActionResult> AssignPermisos(int id, [FromBody] List<string> permisos)
            => Ok(await _service.AsignarPermisos(id, permisos));
    }
}