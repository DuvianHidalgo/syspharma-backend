using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.API.Services;

namespace Syspharma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SyspharmaContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IEmailSender _emailSender;

        // Constructor principal (sin UserManager)
        public AuthController(SyspharmaContext context, IConfiguration config, ILogger<AuthController> logger, IMemoryCache cache, IEmailSender emailSender) =>
            (_context, _config, _logger, _cache, _emailSender) = (context, config, logger, cache, emailSender);

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Estado == true);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password))
                return Unauthorized(new { message = "Credenciales incorrectas" });

            var token = GenerarToken(usuario);
            return Ok(new { token, usuario.Nombre, usuario.Email, rol = usuario.Role.Nombre });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            _logger.LogInformation("Register payload: {@dto}", dto);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest(new { message = "Nombre, email y password son obligatorios" });

            if (!await _context.Database.CanConnectAsync())
                return StatusCode(503, new { message = "No se puede conectar a la base de datos. Intenta más tarde." });

            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { message = "El email ya está registrado" });

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = dto.RoleId,
                Documento = string.IsNullOrEmpty(dto.Documento) ? null : dto.Documento,
                TipoDocumento = dto.TipoDocumento,
                Telefono = dto.Telefono,
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            _context.Usuarios.Add(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al guardar usuario");
                return StatusCode(500, new { message = "Error al guardar el usuario en la base de datos" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en Register");
                return StatusCode(500, new { message = "Error inesperado" });
            }

            return Ok(new { message = "Usuario registrado correctamente" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email requerido" });

            // Buscar el usuario por email usando el DbContext (la app no usa Identity actualmente)
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Estado == true);
            if (user == null)
                return BadRequest(new { message = "No existe un usuario con ese email" });

            // Generar código de 6 dígitos seguro
            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            // Guardar el código en la cache en vez de usar UserManager/Identity
            _cache.Set($"recovery_{user.Id}", code, TimeSpan.FromMinutes(15));

            // Crear el mensaje HTML
            var mensajeHtml = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #e0e0e0; border-radius: 10px;'>
                    <h2 style='color: #059669;'>Recuperación de Contraseña</h2>
                    <p>Hola, <strong>{user.Nombre}</strong>.</p>
                    <p>Has solicitado restablecer tu contraseña. Usa el siguiente código en la aplicación:</p>
                    <div style='background-color: #f3f4f6; padding: 15px; font-size: 24px; font-weight: bold; text-align: center; letter-spacing: 5px; border-radius: 5px;'>
                        {code}
                    </div>
                    <p style='font-size: 12px; color: #6b7280; margin-top: 20px;'>Si no fuiste tú, ignora este mensaje.</p>
                </div>";

            // Enviar el correo
            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Tu Código de Recuperación", mensajeHtml);
                _logger.LogInformation("Código de recuperación enviado a {Email} (usuarioId: {Id})", user.Email, user.Id);
                return Ok(new { message = "Código enviado correctamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando correo de recuperación a {Email}", user.Email);
                return BadRequest($"Error enviando correo: {ex.Message}");
            }
        }

        private string GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role.Nombre)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])) ,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterDto
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public string? Documento { get; set; }

        [JsonPropertyName("tipoDocumento")]
        public string? TipoDocumento { get; set; }

        public string? Telefono { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; } = null!;
    }