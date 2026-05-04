using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Syspharma.Business.Services;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;

namespace Syspharma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly UserManager<Usuario> _userManager;
        private readonly SyspharmaContext _context;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService service, UserManager<Usuario> userManager, SyspharmaContext context, ILogger<UsuarioController> logger)
        {
            _service = service;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarios = await _service.ObtenerTodos();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var usuario = await _service.ObtenerPorId(id);
            if (usuario == null) return NotFound(new { message = "Usuario no encontrado" });
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuarioCreateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Contrasena))
                    return BadRequest(new { message = "La contraseña es obligatoria" });

                if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                    return BadRequest(new { message = "El email ya está registrado" });

                var usuario = new Usuario
                {
                    Nombre = dto.Nombre,
                    Email = dto.Email,
                    UserName = dto.Email,
                    TipoDocumentoId = dto.TipoDocumentoId,
                    Documento = dto.Documento,
                    Telefono = dto.Telefono,
                    RoleId = dto.RolId,
                    FechaCreacion = DateTime.Now,
                    Estado = dto.Estado,
                };

                var resultado = await _userManager.CreateAsync(usuario, dto.Contrasena);
                if (!resultado.Succeeded)
                    return BadRequest(new { message = string.Join(", ", resultado.Errors.Select(e => e.Description)) });

                await _context.Entry(usuario).Reference(u => u.Role).LoadAsync();
                await _context.Entry(usuario).Reference(u => u.TipoDocumento).LoadAsync();

                return Ok(new UsuarioDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Documento = usuario.Documento,
                    TipoDocumento = usuario.TipoDocumento?.Nombre,
                    TipoDocumentoId = usuario.TipoDocumentoId,
                    Telefono = usuario.Telefono,
                    RolNombre = usuario.Role?.Nombre ?? "",
                    Avatar = usuario.Avatar,
                    Estado = usuario.Estado,
                    RolId = usuario.RoleId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] UsuarioUpdateDto dto)
        {
            try
            {
                var usuario = await _service.Actualizar(dto);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool estado)
        {
            try
            {
                var usuario = await _service.CambiarEstado(id, estado);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _service.Eliminar(id);
                if (!ok) return NotFound(new { message = "Usuario no encontrado" });
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario {UserId}: {Inner}", id, ex.InnerException?.Message);
                return Conflict(new
                {
                    message = "No se puede eliminar el usuario por restricciones referenciales.",
                    detail = ex.InnerException?.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar DELETE /api/Usuario/{UserId}", id);
                return BadRequest(new { message = ex.Message, detail = ex.InnerException?.Message });
            }
        }
    }
}