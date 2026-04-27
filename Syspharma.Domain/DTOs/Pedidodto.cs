using System;
using System.Collections.Generic;

namespace Syspharma.Domain.DTOs
{
    public class PedidoDetalleDto
    {
        public int Id { get; set; }
        public int? ProductoId { get; set; }
        public string Nombre { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class PedidoDto
    {
        public int Id { get; set; }
        public string NumeroPedido { get; set; } = null!;
        public int? UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public string ClienteNombre { get; set; } = null!;
        public string? ClienteDocumento { get; set; }
        public string? ClienteTelefono { get; set; }
        public string? ClienteEmail { get; set; }
        public int? MetodoPagoId { get; set; }
        public string? MetodoPagoNombre { get; set; }
        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string? Notas { get; set; }
        public string? Origen { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public List<PedidoDetalleDto> Detalles { get; set; } = new();
    }

    public class PedidoDetalleCreateDto
    {
        public int? ProductoId { get; set; }
        public string Nombre { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class PedidoCreateDto
    {
        public int? UsuarioId { get; set; }
        public string ClienteNombre { get; set; } = null!;
        public string? ClienteDocumento { get; set; }
        public string? ClienteTelefono { get; set; }
        public string? ClienteEmail { get; set; }
        public int? MetodoPagoId { get; set; } // Opcional para evitar error 400
        public decimal PorcentajeIva { get; set; } = 0;
        public string? Notas { get; set; }
        public string? Origen { get; set; } = "web";
        public DateTime? FechaEntrega { get; set; }
        public List<PedidoDetalleCreateDto> Detalles { get; set; } = new();
    }

    public class PedidoUpdateDto
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; } = null!;
        public string? ClienteDocumento { get; set; }
        public string? ClienteTelefono { get; set; }
        public string? ClienteEmail { get; set; }
        public int? MetodoPagoId { get; set; }
        public int EstadoId { get; set; }
        public string? Notas { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public List<PedidoDetalleCreateDto>? Detalles { get; set; }
    }
}