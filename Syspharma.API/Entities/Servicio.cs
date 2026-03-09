using System;
using System.Collections.Generic;

namespace Syspharma.API.Entities;

public partial class Servicio
{
    public int Id { get; set; }

    public int? CitaId { get; set; }

    public int MedicoId { get; set; }

    public string PacienteNombre { get; set; } = null!;

    public decimal Monto { get; set; }

    public int? TurnoId { get; set; }

    public DateTime? FechaServicio { get; set; }

    public string? MedicoNombre { get; set; }

    public string? PacienteDocumento { get; set; }

    public string? ServicioNombre { get; set; }

    public string? Notas { get; set; }

    public virtual Cita? Cita { get; set; }

    public virtual Medico Medico { get; set; } = null!;

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();

    public virtual Turno? Turno { get; set; }
}
