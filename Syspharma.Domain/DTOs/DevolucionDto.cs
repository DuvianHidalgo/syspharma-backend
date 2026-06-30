using System;
using System.Collections.Generic;

namespace Syspharma.Domain.DTOs
{
    public class DevolucionDto
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public string NumeroVenta { get; set; } = "";
        public string ClienteNombre { get; set; } = "";
        public string ClienteDocumento { get; set; } = "";
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = "";
        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; } = "";
        public string Motivo { get; set; } = "";
        public string? Observaciones { get; set; }
        public decimal TotalDevolucion { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public DateTime? FechaGestion { get; set; }
        public int? UsuarioGestionId { get; set; }
        public List<DetalleDevolucionDto> Detalles { get; set; } = new();
    }

    public class DetalleDevolucionDto
    {
        public int Id { get; set; }
        public int DevolucionId { get; set; }
        public int DetalleVentaId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = "";
        public int CantidadDevuelta { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubtotalDevuelto { get; set; }
    }

    public class DevolucionCreateDto
    {
        public int VentaId { get; set; }
        public int UsuarioId { get; set; }
        public string Motivo { get; set; } = "";
        public string? Observaciones { get; set; }
        public List<DetalleDevolucionCreateDto> Detalles { get; set; } = new();
    }

    public class DetalleDevolucionCreateDto
    {
        public int DetalleVentaId { get; set; }
        public int ProductoId { get; set; }
        public int CantidadDevuelta { get; set; }
    }

    public class DevolucionGestionarDto
    {
        public int NuevoEstado { get; set; }      // 2 = Aprobada | 3 = Rechazada
        public int UsuarioGestionId { get; set; }
    }

    public class EstadoDevolucionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public bool Activo { get; set; }
    }
}
