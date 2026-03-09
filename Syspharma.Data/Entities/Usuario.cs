using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Syspharma.Data.Entities;

public partial class Usuario : IdentityUser<int>
{
    public string Nombre { get; set; } = null!;
    public string? Documento { get; set; }
    public string? TipoDocumento { get; set; }
    public string? Telefono { get; set; }
    public int RoleId { get; set; }
    public string? Avatar { get; set; }
    public bool? Estado { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? UltimoAcceso { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = [];
    public virtual ICollection<Gasto> Gastos { get; set; } = [];
    public virtual Role Role { get; set; } = null!;
    public virtual ICollection<Turno> Turnos { get; set; } = [];
    public virtual ICollection<Venta> Venta { get; set; } = [];
}