using System;
using System.Collections.Generic;

namespace Syspharma.Data.Entities;

public partial class Venta
{
    public int Id { get; set; }

    public string NumeroVenta { get; set; } = null!;

    public int TurnoId { get; set; }

    public int UsuarioId { get; set; }

    public string? ClienteNombre { get; set; }

    public string? ClienteDocumento { get; set; }

    public string? ClienteTelefono { get; set; }

    public DateTime? FechaVenta { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Impuesto { get; set; }

    public decimal Total { get; set; }

    public string? MetodoPago { get; set; }

    public string? Estado { get; set; }

    public string? Notas { get; set; }

    public virtual Turno Turno { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
