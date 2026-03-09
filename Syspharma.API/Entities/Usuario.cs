using System;
using System.Collections.Generic;

namespace Syspharma.API.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Documento { get; set; }

    public string? TipoDocumento { get; set; }

    public string? Telefono { get; set; }

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public string? Avatar { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimoAcceso { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual ICollection<Pedido> PedidoClientes { get; set; } = new List<Pedido>();

    public virtual ICollection<Pedido> PedidoEmpleados { get; set; } = new List<Pedido>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
