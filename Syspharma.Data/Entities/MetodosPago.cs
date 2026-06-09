using System;
using System.Collections.Generic;

namespace Syspharma.Data.Entities;

public partial class MetodosPago
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();

    // AGREGAR ESTA LÍNEA:
    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}