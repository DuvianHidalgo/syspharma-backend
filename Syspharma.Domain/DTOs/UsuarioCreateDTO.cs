namespace Syspharma.Domain.DTOs
{
    public class UsuarioCreateDto
    {
        public int? TipoDocumentoId { get; set; }
        public string? Documento { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Telefono { get; set; }
        public int RolId { get; set; }
        public bool Estado { get; set; }
        public string Contrasena { get; set; } = null!;
    }
}