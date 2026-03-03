using System;
using System.Collections.Generic;

namespace Syspharma.Data.Entities;

public partial class Servicio
{
    public int Id { get; set; }

    public int? CitaId { get; set; }

    public int MedicoId { get; set; }

    public string MedicoNombre { get; set; } = null!;

    public string PacienteNombre { get; set; } = null!;

    public string? PacienteDocumento { get; set; }

    public string? ServicioNombre { get; set; }

    public decimal Monto { get; set; }

    public int? TurnoId { get; set; }

    public DateTime? FechaServicio { get; set; }

    public string? Notas { get; set; }

    public virtual Cita? Cita { get; set; }

    public virtual Medico Medico { get; set; } = null!;

    public virtual Turno? Turno { get; set; }
}
