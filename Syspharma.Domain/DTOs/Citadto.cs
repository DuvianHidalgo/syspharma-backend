using System;
using System.Collections.Generic;

namespace Syspharma.Domain.DTOs
{
    // DTO para los selectores de estado
    public class CitaEstadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }

    public class CitaDto
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public string MedicoNombre { get; set; } = null!;
        public string PacienteNombre { get; set; } = null!;
        public string? PacienteDocumento { get; set; }
        public string? PacienteTelefono { get; set; }
        public string? PacienteEmail { get; set; }
        public int? ServicioId { get; set; }
        public string? ServicioNombre { get; set; }
        public decimal? Precio { get; set; }
        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; } = null!;
        public int? UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public int? PedidoId { get; set; }
        public int? VentaId { get; set; }
        public string Fecha { get; set; } = null!;
        public string Hora { get; set; } = null!;
        public string? Notas { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class CitaCreateDto
    {
        public int MedicoId { get; set; }
        public string PacienteNombre { get; set; } = null!;
        public string? PacienteDocumento { get; set; }
        public string? PacienteTelefono { get; set; }
        public string? PacienteEmail { get; set; }
        public int? ServicioId { get; set; }
        public int? UsuarioId { get; set; }
        public string Fecha { get; set; } = null!;
        public string Hora { get; set; } = null!;
        public string? Notas { get; set; }
    }

    public class CitaUpdateDto
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public string PacienteNombre { get; set; } = null!;
        public string? PacienteDocumento { get; set; }
        public string? PacienteTelefono { get; set; }
        public string? PacienteEmail { get; set; }
        public int? ServicioId { get; set; }
        public int EstadoId { get; set; }
        public string Fecha { get; set; } = null!;
        public string Hora { get; set; } = null!;
        public string? Notas { get; set; }
    }
}