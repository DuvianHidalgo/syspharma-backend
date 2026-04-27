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
    public class GastoController : ControllerBase
    {
        private readonly IGastoService _service;
        public GastoController(IGastoService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() => Ok(await _service.ObtenerTodos());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var g = await _service.ObtenerPorId(id);
            return g == null ? NotFound(new { message = "Gasto no encontrado" }) : Ok(g);
        }

        [HttpGet("turno/{turnoId}")]
        public async Task<IActionResult> ObtenerPorTurno(int turnoId) =>
            Ok(await _service.ObtenerPorTurno(turnoId));

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] GastoCreateDto dto)
        {
            try { return Ok(await _service.Crear(dto)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] GastoUpdateDto dto)
        {
            try { return Ok(await _service.Actualizar(dto)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try { await _service.Eliminar(id); return Ok(new { message = "Gasto eliminado correctamente" }); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}