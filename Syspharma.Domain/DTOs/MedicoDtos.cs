namespace Syspharma.Domain.DTOs
{
    public class MedicoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Especialidad { get; set; }
        public string? Documento { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? DiasLaborales { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraFin { get; set; }
        public int? Intervalo { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class MedicoCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string? Especialidad { get; set; }
        public string? Documento { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? DiasLaborales { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraFin { get; set; }
        public int? Intervalo { get; set; }
    }

    public class MedicoUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Especialidad { get; set; }
        public string? Documento { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? DiasLaborales { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraFin { get; set; }
        public int? Intervalo { get; set; }
    }
}