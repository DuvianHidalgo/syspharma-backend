namespace Syspharma.Domain.DTOs
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Contacto { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public int? TipoDocumentoId { get; set; }
        public string? TipoDocumento { get; set; }
        public string? Documento { get; set; }
        public int? EstadoId { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class ProveedorCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string? Contacto { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public int? TipoDocumentoId { get; set; }
        public string? Documento { get; set; }
    }

    public class ProveedorUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Contacto { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public int? TipoDocumentoId { get; set; }
        public string? Documento { get; set; }
    }
}