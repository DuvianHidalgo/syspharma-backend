namespace Syspharma.Domain.DTOs
{
    public class UsuarioUpdateDto
    {
        public int Id { get; set; }
        public int? TipoDocumentoId { get; set; }
        public string? Documento { get; set; }
        public string? Nombre { get; set; } = null;
        public string? Apellidos { get; set; } = null;
        public string Email { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public int RolId { get; set; }
        public bool Estado { get; set; }
    }
}