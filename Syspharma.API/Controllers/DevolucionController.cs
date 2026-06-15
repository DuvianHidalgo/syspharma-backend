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
    public class DevolucionController : ControllerBase
    {
        private readonly IDevolucionService _service;
        public DevolucionController(IDevolucionService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var devoluciones = await _service.ObtenerTodos();
            return Ok(devoluciones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var devolucion = await _service.ObtenerPorId(id);
            return devolucion == null
                ? NotFound(new { message = "Devolución no encontrada" })
                : Ok(devolucion);
        }

        // Endpoint para que el front cargue los productos
        // de una venta antes de registrar la devolución
        [HttpGet("venta/{ventaId}")]
        public async Task<IActionResult> ObtenerVentaParaDevolucion(int ventaId)
        {
            var venta = await _service.ObtenerVentaParaDevolucion(ventaId);
            return venta == null
                ? NotFound(new { message = "Venta no encontrada" })
                : Ok(venta);
        }

        [HttpGet("estados")]
        public async Task<IActionResult> ObtenerEstados()
        {
            var estados = await _service.ObtenerEstados();
            return Ok(estados);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] DevolucionCreateDto dto)
        {
            try
            {
                var devolucion = await _service.Crear(dto);
                return Ok(devolucion);
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // Aprobar o rechazar — body: { nuevoEstado: 2|3, usuarioGestionId: X }
        [HttpPatch("{id}/gestionar")]
        public async Task<IActionResult> Gestionar(int id, [FromBody] DevolucionGestionarDto dto)
        {
            try
            {
                await _service.Gestionar(id, dto);
                return Ok(new { message = "Devolución gestionada correctamente" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}