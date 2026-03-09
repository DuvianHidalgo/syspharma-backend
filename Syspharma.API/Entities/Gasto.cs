using System;
using System.Collections.Generic;

namespace Syspharma.API.Entities;

public partial class Gasto
{
    public int Id { get; set; }

    public int TurnoId { get; set; }

    public int UsuarioId { get; set; }

    public string Concepto { get; set; } = null!;

    public decimal Monto { get; set; }

    public DateTime? FechaGasto { get; set; }

    public string? Categoria { get; set; }

    public string? Descripcion { get; set; }

    public string? Comprobante { get; set; }

    public virtual Turno Turno { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
