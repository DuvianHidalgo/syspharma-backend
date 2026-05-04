using System;
using System.Collections.Generic;

namespace Syspharma.Domain.DTOs
{
    public class VentaDto
    {
        public int Id { get; set; }
        public string NumeroVenta { get; set; } = null!;
        public int TurnoId { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = "";
        public string? ClienteNombre { get; set; }
        public string? ClienteDocumento { get; set; }
        public string? ClienteTelefono { get; set; }
        public int MetodoPagoId { get; set; }
        public string MetodoPagoNombre { get; set; } = "";
        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; } = "";
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal PorcentajeIva { get; set; }
        public decimal Total { get; set; }
        public string? Notas { get; set; }
        public DateTime? FechaVenta { get; set; }
        public List<VentaDetalleDto> Detalles { get; set; } = new();
        public List<VentaDetalleServicioDto> Servicios { get; set; } = new();
    }

    public class VentaDetalleDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string? ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class VentaDetalleServicioDto
    {
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public string? ServicioNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class VentaCreateDto
    {
        public int TurnoId { get; set; }
        public int UsuarioId { get; set; }
        public string? ClienteNombre { get; set; }
        public string? ClienteDocumento { get; set; }
        public string? ClienteTelefono { get; set; }
        public int MetodoPagoId { get; set; }
        public decimal PorcentajeIva { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string? Notas { get; set; }
        public List<VentaDetalleCreateDto> Detalles { get; set; } = new();
        public List<VentaDetalleServicioCreateDto> Servicios { get; set; } = new();
    }

    public class VentaDetalleCreateDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class VentaDetalleServicioCreateDto
    {
        public int ServicioId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class VentaUpdateDto
    {
        public int Id { get; set; }
        public string? ClienteNombre { get; set; }
        public string? ClienteDocumento { get; set; }
        public string? ClienteTelefono { get; set; }
        public int MetodoPagoId { get; set; }
        public int EstadoId { get; set; }
        public string? Notas { get; set; }
    }

    public class EstadoVentaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}