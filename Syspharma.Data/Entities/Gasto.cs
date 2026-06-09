using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Syspharma.Data.Entities;
public partial class Gasto
{
    public int Id { get; set; }
    public int TurnoId { get; set; }
    public int UsuarioId { get; set; }
    public string? NumeroGasto { get; set; }
    public string Concepto { get; set; } = null!;
    public string? Descripcion { get; set; }
    public decimal Monto { get; set; }
    public string Categoria { get; set; } = null!;
    [Column("metodoPagoId")]
    public int? MetodoPagoId { get; set; }
    public int? EstadoId { get; set; }
    public decimal? Subtotal { get; set; }
    public decimal? Iva { get; set; }
    public decimal? PorcentajeIva { get; set; }
    public string? Notas { get; set; }
    public string? ProveedorNombre { get; set; }
    public string? ProveedorDocumento { get; set; }
    public string? ProveedorTelefono { get; set; }
    public string? Comprobante { get; set; }
    public DateTime? FechaGasto { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public virtual Turno Turno { get; set; } = null!;
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual MetodosPago? MetodoPago { get; set; }
}