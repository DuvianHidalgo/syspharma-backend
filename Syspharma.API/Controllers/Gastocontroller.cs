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

        // NUEVO: Gastos de hoy
        [HttpGet("today")]
        public async Task<IActionResult> ObtenerHoy([FromQuery] int? usuarioId = null)
        {
            try
            {
                var result = await _service.ObtenerHoy(usuarioId);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // NUEVO: KPIs
        [HttpGet("kpis")]
        public async Task<IActionResult> ObtenerKpis([FromQuery] DateTime? fecha = null)
        {
            try
            {
                var result = await _service.ObtenerKpis(fecha);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] GastoCreateDto dto)
        {
            try
            {
                return Ok(await _service.Crear(dto));
            }
            catch (Exception ex)
            {
                // Devolver el INNER EXCEPTION (el error real de SQL)
                var errorReal = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { message = errorReal });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] GastoUpdateDto dto)
        {
            try { return Ok(await _service.Actualizar(dto)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // NUEVO: Anular
        [HttpPut("{id}/anular")]
        public async Task<IActionResult> Anular(int id, [FromBody] string notas = null)
        {
            try
            {
                await _service.Anular(id, notas);
                return Ok(new { success = true, message = "Gasto anulado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try { await _service.Eliminar(id); return Ok(new { message = "Gasto eliminado correctamente" }); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}