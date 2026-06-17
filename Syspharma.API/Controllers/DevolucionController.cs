<<<<<<< Updated upstream
﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
=======
using Microsoft.AspNetCore.Authentication.JwtBearer;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        public async Task<IActionResult> ObtenerTodos()
        {
            var devoluciones = await _service.ObtenerTodos();
            return Ok(devoluciones);
=======
        public async Task<IActionResult> ObtenerTodos() =>
            Ok(await _service.ObtenerTodos());

        [HttpGet("estados")]
        public async Task<IActionResult> ObtenerEstados() =>
            Ok(await _service.ObtenerEstados());

        [HttpGet("venta/{ventaId}")]
        public async Task<IActionResult> ObtenerPorVenta(int ventaId)
        {
            var result = await _service.ObtenerPorVentaId(ventaId);
            return result == null ? NotFound(new { message = "No hay devolución para esta venta" }) : Ok(result);
>>>>>>> Stashed changes
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
<<<<<<< Updated upstream
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
=======
            var result = await _service.ObtenerPorId(id);
            return result == null ? NotFound(new { message = "Devolución no encontrada" }) : Ok(result);
>>>>>>> Stashed changes
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] DevolucionCreateDto dto)
        {
<<<<<<< Updated upstream
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
=======
            try { return Ok(await _service.Crear(dto)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPatch("{id}/gestionar")]
        public async Task<IActionResult> Gestionar(int id, [FromBody] DevolucionGestionarDto dto)
        {
            try { return Ok(await _service.Gestionar(id, dto)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }
    }
}
>>>>>>> Stashed changes
