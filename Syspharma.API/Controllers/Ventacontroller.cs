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
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _service;
        public VentaController(IVentaService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var ventas = await _service.ObtenerTodos();
            return Ok(ventas);
        }

        [HttpGet("estados")]
        public async Task<IActionResult> ObtenerEstados()
        {
            var estados = await _service.ObtenerEstados();
            return Ok(estados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var venta = await _service.ObtenerPorId(id);
            return venta == null ? NotFound(new { message = "Venta no encontrada" }) : Ok(venta);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VentaCreateDto dto)
        {
            try
            {
                var venta = await _service.Crear(dto);
                return Ok(venta);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VentaUpdateDto dto)
        {
            try
            {
                var venta = await _service.Actualizar(dto);
                return Ok(venta);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] int estadoId)
        {
            try
            {
                await _service.CambiarEstado(id, estadoId);
                return Ok(new { message = "Estado actualizado correctamente" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPatch("{id}/anular")]
        public async Task<IActionResult> Anular(int id)
        {
            try
            {
<<<<<<< Updated upstream
                await _service.Anular(id);
=======
                var result = await _service.Anular(id);
                if (!result) return NotFound(new { message = "Venta no encontrada" });
>>>>>>> Stashed changes
                return Ok(new { message = "Venta anulada correctamente" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _service.Eliminar(id);
                return Ok(new { message = "Venta eliminada correctamente" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
