using System;

namespace Syspharma.Domain.DTOs
{
    public class CompraDetalleDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public string? Lote { get; set; }
        public DateOnly? FechaVencimiento { get; set; }
    }
    public class CompraDto
    {
        public int Id { get; set; }
        public string NumeroCompra { get; set; } = null!;
        public int ProveedorId { get; set; }
        public string ProveedorNombre { get; set; } = null!;
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = null!;
        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string? Notas { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaCompra { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public List<CompraDetalleDto> Detalles { get; set; } = new();
    }
    public class CompraDetalleCreateDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string? Lote { get; set; }
        public DateOnly? FechaVencimiento { get; set; }
    }
    public class CompraCreateDto
    {
        public int ProveedorId { get; set; }
        public int UsuarioId { get; set; }
        public decimal? PorcentajeIva { get; set; } = 19;
        public string? Notas { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public List<CompraDetalleCreateDto> Detalles { get; set; } = new();
    }
    public class CompraUpdateDto
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public int EstadoId { get; set; }
        public string? Notas { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public decimal PorcentajeIva { get; set; } = 19;
        public List<CompraDetalleCreateDto> Detalles { get; set; } = new();
    }
}
