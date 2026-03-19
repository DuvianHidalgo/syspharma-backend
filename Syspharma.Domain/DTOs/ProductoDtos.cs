namespace Syspharma.Domain.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
        public int? ProveedorId { get; set; }
        public string? ProveedorNombre { get; set; }
        public decimal Precio { get; set; }
        public decimal? PrecioCompra { get; set; }
        public int? Stock { get; set; }
        public string? Sku { get; set; }
        public string? CodigoBarras { get; set; }
        public string? Imagen { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? UltimaActualizacion { get; set; }
    }

    public class ProductoCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public int? ProveedorId { get; set; }
        public decimal Precio { get; set; }
        public decimal? PrecioCompra { get; set; }
        public int? Stock { get; set; }
        public string? Sku { get; set; }
        public string? CodigoBarras { get; set; }
        public string? Imagen { get; set; }
    }

    public class ProductoUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public int? ProveedorId { get; set; }
        public decimal Precio { get; set; }
        public decimal? PrecioCompra { get; set; }
        public int? Stock { get; set; }
        public string? Sku { get; set; }
        public string? CodigoBarras { get; set; }
        public string? Imagen { get; set; }
    }
}