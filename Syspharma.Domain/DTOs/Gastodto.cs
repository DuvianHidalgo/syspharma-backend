namespace Syspharma.Domain.DTOs
{
    public class GastoDto
    {
        public int Id { get; set; }
        public int TurnoId { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = null!;
        public string Concepto { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Categoria { get; set; } = null!;
        public string? Comprobante { get; set; }
        public DateTime? FechaGasto { get; set; }
    }

    public class GastoCreateDto
    {
        public int TurnoId { get; set; }
        public int UsuarioId { get; set; }
        public string Concepto { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Categoria { get; set; } = "operacional";
        public string? Comprobante { get; set; }
    }

    public class GastoUpdateDto
    {
        public int Id { get; set; }
        public string Concepto { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Categoria { get; set; } = null!;
        public string? Comprobante { get; set; }
    }
}