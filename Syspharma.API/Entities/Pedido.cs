using System;
using System.Collections.Generic;

namespace Syspharma.API.Entities;

public partial class Pedido
{
    public int Id { get; set; }

    public string NumeroPedido { get; set; } = null!;

    public int? ClienteId { get; set; }

    public int? EmpleadoId { get; set; }

    public string Origen { get; set; } = null!;

    public string? Estado { get; set; }

    public decimal Total { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public virtual Usuario? Cliente { get; set; }

    public virtual Usuario? Empleado { get; set; }

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
