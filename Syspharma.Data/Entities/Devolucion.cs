using Syspharma.Data.Entities;

namespace Syspharma.Data.Entities
{
    public class Devolucion
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int UsuarioId { get; set; }
        public int EstadoId { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public decimal TotalDevolucion { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public DateTime? FechaGestion { get; set; }
        public int? UsuarioGestionId { get; set; }

        // Navegación
        public virtual Venta Venta { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
        public virtual EstadoDevolucion Estado { get; set; } = null!;
        public virtual ICollection<DetalleDevolucion> Detalles { get; set; } = new List<DetalleDevolucion>();
    }
}