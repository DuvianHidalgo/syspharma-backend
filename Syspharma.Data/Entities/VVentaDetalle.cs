using System;
using System.Collections.Generic;

namespace Syspharma.Data.Entities;

public partial class VVentaDetalle
{
    public int Id { get; set; }

    public int VentaId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal? Descuento { get; set; }

    public decimal? SubtotalCalculado { get; set; }
}
