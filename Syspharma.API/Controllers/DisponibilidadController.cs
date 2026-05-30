using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Syspharma.Business.Services;
using Syspharma.Domain.DTOs;

namespace Syspharma.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DisponibilidadController : ControllerBase
{
    private readonly IDisponibilidadService _service;
    public DisponibilidadController(IDisponibilidadService service) => _service = service;

    // GET api/Disponibilidad/horario/5
    [HttpGet("horario/{medicoId}")]
    public async Task<IActionResult> ObtenerHorario(int medicoId)
        => Ok(await _service.ObtenerHorario(medicoId));

    // POST api/Disponibilidad/horario
    // Body: { medicoId, horarios: [{diaSemana, mananaInicio, ...}] }
    [HttpPost("horario")]
    public async Task<IActionResult> GuardarHorario([FromBody] GuardarHorarioDto dto)
    {
        await _service.GuardarHorario(dto.MedicoId, dto.Horarios);
        return Ok(new { message = "Horario guardado correctamente" });
    }

    // GET api/Disponibilidad/slots/5?fecha=2026-05-29
    [HttpGet("slots/{medicoId}")]
    public async Task<IActionResult> ObtenerSlots(int medicoId, [FromQuery] string fecha)
        => Ok(await _service.ObtenerSlots(medicoId, fecha));

    // GET api/Disponibilidad/dias-no-disponibles/5
    [HttpGet("dias-no-disponibles/{medicoId}")]
    public async Task<IActionResult> ObtenerDiasNoDisponibles(int medicoId)
        => Ok(await _service.ObtenerDiasNoDisponibles(medicoId));

    // POST api/Disponibilidad/dias-no-disponibles
    [HttpPost("dias-no-disponibles")]
    public async Task<IActionResult> AgregarDiaNoDisponible([FromBody] DiaNoDisponibleCreateDto dto)
    {
        var result = await _service.AgregarDiaNoDisponible(dto);
        return Ok(result);
    }

    // DELETE api/Disponibilidad/dias-no-disponibles/3
    [HttpDelete("dias-no-disponibles/{id}")]
    public async Task<IActionResult> EliminarDiaNoDisponible(int id)
    {
        await _service.EliminarDiaNoDisponible(id);
        return Ok(new { message = "Eliminado correctamente" });
    }
}