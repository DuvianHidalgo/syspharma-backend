<<<<<<< Updated upstream
﻿using System;

=======
>>>>>>> Stashed changes
namespace Syspharma.Domain.DTOs
{
    public class GastoDto
    {
        public int Id { get; set; }
        public int TurnoId { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = null!;
        public string? NumeroGasto { get; set; }
        public string Concepto { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Categoria { get; set; } = null!;
        public int? MetodoPagoId { get; set; }
        public string? MetodoPago { get; set; }
        public int? EstadoId { get; set; }
        public string? Estado { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Iva { get; set; }
        public decimal? PorcentajeIva { get; set; }
        public string? Notas { get; set; }
        public string? Proveedor { get; set; }
        public string? Comprobante { get; set; }
        public string? ComprobanteUrl { get; set; }
        public DateTime? FechaGasto { get; set; }
<<<<<<< Updated upstream
        public DateTime? FechaCreacion { get; set; }

        // Campos específicos para el frontend (ExpensesModal)
        public string? Hora { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Observaciones { get; set; }
=======
        public bool Anulado { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public string? MotivoAnulacion { get; set; }
>>>>>>> Stashed changes
    }

    public class GastoCreateDto
    {
        public int TurnoId { get; set; }
        public int UsuarioId { get; set; }
        public string? NumeroGasto { get; set; }
        public string Concepto { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Categoria { get; set; } = "operacional";
        public int? MetodoPagoId { get; set; }
        public string? Notas { get; set; }
        public string? Comprobante { get; set; }
        public DateTime? FechaGasto { get; set; }
    }

    public class GastoUpdateDto
    {
        public int Id { get; set; }
        public string? NumeroGasto { get; set; }
        public string Concepto { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Categoria { get; set; } = null!;
        public int? MetodoPagoId { get; set; }
        public string? Notas { get; set; }
        public string? Comprobante { get; set; }
        public DateTime? FechaGasto { get; set; }
    }

    // NUEVO: DTO para KPIs
    public class GastoKpiDto
    {
        public decimal TotalGastosDia { get; set; }
        public int CantidadGastosDia { get; set; }
        public decimal TotalNomina { get; set; }
        public decimal TotalServicios { get; set; }
        public decimal TotalMantenimiento { get; set; }
    }
}
