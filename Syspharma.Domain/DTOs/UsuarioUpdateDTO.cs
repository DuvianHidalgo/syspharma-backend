namespace Syspharma.Domain.DTOs
{
    public class UsuarioUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Apellidos { get; set; }        // ✅ Agregado
        public string Email { get; set; } = null!;
        public string? Documento { get; set; }
        public int? TipoDocumentoId { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }        // ✅ Agregado
        public string? Avatar { get; set; }           // ✅ Agregado (para foto)
        public int RolId { get; set; }
        public bool Estado { get; set; }
        public decimal PorcentajeIva { get; set; } = 19m; // ✅ Agregado, valor por defecto 19%
    }
}