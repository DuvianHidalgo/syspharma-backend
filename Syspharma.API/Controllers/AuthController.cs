using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;

namespace Syspharma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SyspharmaContext _Context;
        private readonly IConfiguration _config;

        public AuthController(SyspharmaContext Context, IConfiguration config)
        {
            _Context = Context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var usuario = await _Context.Usuarios
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
    if (await _Context.Usuarios.AnyAsync(u => u.Email == dto.Email))
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

    _Context.Usuarios.Add(usuario);
    await _Context.SaveChangesAsync();

    return Ok(new { message = "Usuario registrado correctamente" });
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
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])),
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
        public string? TipoDocumento { get; set; }
        public string? Telefono { get; set; }
    }
}