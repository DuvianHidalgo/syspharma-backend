using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;

namespace Syspharma.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SyspharmaContext _context;

        public UsuarioRepository(SyspharmaContext context)
        {
            _context = context;
        }

        public async Task<UsuarioDto> Actualizar(UsuarioUpdateDto dto)
        {
            if (!await _context.Usuarios.AnyAsync(u => u.Id == dto.Id))
                throw new Exception("Usuario no encontrado");

            var usuario = await _context.Usuarios
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (usuario == null) return null;
            else
            {
                usuario.Nombre = dto.Nombre;
                usuario.Email = dto.Email;
                usuario.TipoDocumento = dto.TipoDocumento;
                usuario.Documento = dto.Documento;
                usuario.Telefono = dto.Telefono;
                usuario.RoleId = dto.RolId;
                usuario.Estado = dto.Estado;
            }

            await _context.SaveChangesAsync();

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Documento = usuario.Documento,
                TipoDocumento = usuario.TipoDocumento,
                Telefono = usuario.Telefono,
                RolNombre = usuario.Role?.Nombre ?? "",
                Avatar = usuario.Avatar,
                Estado = usuario.Estado ?? true,
                RolId = usuario.RoleId
            };
        }

        public async Task<UsuarioDto> CambiarEstado(int id, bool estado)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.Estado = estado;
            await _context.SaveChangesAsync();

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Documento = usuario.Documento,
                TipoDocumento = usuario.TipoDocumento,
                Telefono = usuario.Telefono,
                RolNombre = usuario.Role?.Nombre ?? "",
                Avatar = usuario.Avatar,
                Estado = usuario.Estado ?? true,
                RolId = usuario.RoleId
            };
        }

        public async Task<UsuarioDto> Crear(UsuarioCreateDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("El email ya está registrado");

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                TipoDocumento = dto.TipoDocumento,
                Documento = dto.Documento,
                Telefono = dto.Telefono,
                RoleId = dto.RolId,
                FechaCreacion = DateTime.Now,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                Estado = true,
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Documento = usuario.Documento,
                TipoDocumento = usuario.TipoDocumento,
                Telefono = usuario.Telefono,
                RolNombre = usuario.Role?.Nombre ?? "",
                Avatar = usuario.Avatar,
                Estado = usuario.Estado ?? true,
                RolId = usuario.RoleId
            };
        }

        public async Task<UsuarioDto?> ObtenerPorId(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id && u.Estado == true);
            if (usuario == null) return null;
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Documento = usuario.Documento,
                TipoDocumento = usuario.TipoDocumento,
                Telefono = usuario.Telefono,
                RolNombre = usuario.Role?.Nombre ?? "",
                Avatar = usuario.Avatar,
                Estado = usuario.Estado ?? true,
                RolId = usuario.RoleId
            };
        }

        public async Task<List<UsuarioDto>> ObtenerTodos()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Role)
                .ToListAsync();
            return usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                Documento = u.Documento,
                TipoDocumento = u.TipoDocumento,
                Telefono = u.Telefono,
                RolNombre = u.Role?.Nombre ?? "",
                Avatar = u.Avatar,
                Estado = u.Estado ?? true,
                RolId = u.RoleId
            }).ToList();
        }
    }
}