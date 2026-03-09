using System;
using System.Collections.Generic;

namespace Syspharma.API.Entities;

public partial class Carrito
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<CarritoDetalle> CarritoDetalles { get; set; } = new List<CarritoDetalle>();

    public virtual Usuario Usuario { get; set; } = null!;
}
