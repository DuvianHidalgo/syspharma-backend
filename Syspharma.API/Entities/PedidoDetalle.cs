using System;
using System.Collections.Generic;

namespace Syspharma.API.Entities;

public partial class PedidoDetalle
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int? ProductoId { get; set; }

    public int? ServicioId { get; set; }

    public string Tipo { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual Producto? Producto { get; set; }

    public virtual Servicio? Servicio { get; set; }
}
