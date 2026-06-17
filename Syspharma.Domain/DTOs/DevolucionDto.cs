<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;

=======
>>>>>>> Stashed changes
namespace Syspharma.Domain.DTOs
{
    public class DevolucionDto
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
<<<<<<< Updated upstream
        public string NumeroVenta { get; set; } = "";
        public string ClienteNombre { get; set; } = "";
        public string ClienteDocumento { get; set; } = "";
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = "";
        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; } = "";
        public string Motivo { get; set; } = "";
=======
        public string? NumeroVenta { get; set; }
        public int UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public int EstadoId { get; set; }
        public string? EstadoNombre { get; set; }
        public string Motivo { get; set; } = null!;
>>>>>>> Stashed changes
        public string? Observaciones { get; set; }
        public decimal TotalDevolucion { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public DateTime? FechaGestion { get; set; }
<<<<<<< Updated upstream
=======
        public int? UsuarioGestionId { get; set; }
>>>>>>> Stashed changes
        public List<DetalleDevolucionDto> Detalles { get; set; } = new();
    }

    public class DetalleDevolucionDto
    {
        public int Id { get; set; }
<<<<<<< Updated upstream
        public int DevolucionId { get; set; }
        public int DetalleVentaId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = "";
        public int CantidadDevuelta { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubtotalDevuelto { get; set; }
=======
        public int DetalleVentaId { get; set; }
        public int ProductoId { get; set; }
        public string? ProductoNombre { get; set; }
        public int CantidadDevuelta { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? SubtotalDevuelto { get; set; }
>>>>>>> Stashed changes
    }

    public class DevolucionCreateDto
    {
        public int VentaId { get; set; }
        public int UsuarioId { get; set; }
<<<<<<< Updated upstream
        public string Motivo { get; set; } = "";
=======
        public string Motivo { get; set; } = null!;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        public int NuevoEstado { get; set; }      // 2 = Aprobada | 3 = Rechazada
=======
        public int NuevoEstado { get; set; }
>>>>>>> Stashed changes
        public int UsuarioGestionId { get; set; }
    }

    public class EstadoDevolucionDto
    {
        public int Id { get; set; }
<<<<<<< Updated upstream
        public string Nombre { get; set; } = "";
    }
}
=======
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; }
    }
}
>>>>>>> Stashed changes
