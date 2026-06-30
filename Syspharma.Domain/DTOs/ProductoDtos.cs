using System;

namespace Syspharma.Domain.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public string? Presentacion { get; set; }
        public int CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
        public int? ProveedorId { get; set; }
        public string? ProveedorNombre { get; set; }
        public decimal Precio { get; set; }
        public decimal? PrecioCompra { get; set; }
        public int Stock { get; set; }
        public string? CodigoBarras { get; set; }
        public string? Imagen { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? UltimaActualizacion { get; set; }
        public DateOnly? FechaVencimientoProxima { get; set; }

        // --- NUEVAS PROPIEDADES DE MEDICAMENTO ---
        public ProductoMedicamentoDto? Medicamento { get; set; }
        public List<LoteDto> Lotes { get; set; } = new();
    }

    public class ProductoCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public string? Presentacion { get; set; }
        public int CategoriaId { get; set; }
        public int? ProveedorId { get; set; }
        public decimal Precio { get; set; }
        public decimal? PrecioCompra { get; set; }
        public int? Stock { get; set; }
        public string? CodigoBarras { get; set; }
        public string? Imagen { get; set; }

        // --- NUEVAS PROPIEDADES DE MEDICAMENTO ---
        public bool EsMedicamento { get; set; } = false;
        public ProductoMedicamentoDto? Medicamento { get; set; }
    }

    public class ProductoUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public string? Presentacion { get; set; }
        public int CategoriaId { get; set; }
        public int? ProveedorId { get; set; }
        public decimal Precio { get; set; }
        public decimal? PrecioCompra { get; set; }
        public int? Stock { get; set; }
        public string? CodigoBarras { get; set; }
        public string? Imagen { get; set; }

        // --- NUEVAS PROPIEDADES DE MEDICAMENTO ---
        public bool EsMedicamento { get; set; } = false;
        public ProductoMedicamentoDto? Medicamento { get; set; }
    }

    public class LoteDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string NumeroLote { get; set; } = null!;
        public int Cantidad { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        public decimal CostoUnitario { get; set; }
    }
}