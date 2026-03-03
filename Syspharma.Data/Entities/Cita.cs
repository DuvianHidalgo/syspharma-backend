using System;
using System.Collections.Generic;

namespace Syspharma.Data.Entities;

public partial class Cita
{
    public int Id { get; set; }

    public int MedicoId { get; set; }

    public string PacienteNombre { get; set; } = null!;

    public string? PacienteDocumento { get; set; }

    public string? PacienteTelefono { get; set; }

    public string? PacienteEmail { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly Hora { get; set; }

    public string? ServicioNombre { get; set; }

    public decimal? Precio { get; set; }

    public string? Estado { get; set; }

    public string? Notas { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Medico Medico { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
