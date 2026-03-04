using System;
using System.Collections.Generic;

namespace Syspharma.API.Entities;

public partial class VResumenTurno
{
    public int Id { get; set; }

    public string Empleado { get; set; } = null!;

    public DateTime? FechaApertura { get; set; }

    public DateTime? FechaCierre { get; set; }

    public decimal MontoBase { get; set; }

    public decimal TotalVentas { get; set; }

    public decimal TotalGastos { get; set; }

    public string? Estado { get; set; }
}
