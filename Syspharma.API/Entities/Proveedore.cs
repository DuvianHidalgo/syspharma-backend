using System;
using System.Collections.Generic;

namespace Syspharma.API.Entities;

public partial class Proveedore
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Contacto { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public bool? Estado { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}