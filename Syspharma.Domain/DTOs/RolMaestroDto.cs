using System;
using System.Collections.Generic;

namespace Syspharma.Domain.DTOs
{
    public class RolDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        // Lista de códigos de permisos asociados (ej: "users.view", "billing.create")
        public List<string> Permisos { get; set; } = new();
    }

    public class RolCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        // Se pueden enviar los códigos de permisos directamente al crear
        public List<string>? Permisos { get; set; }
    }

    public class RolUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public List<string>? Permisos { get; set; }
    }

    // Útil para poblar los checkboxes en el frontend
    public class PermisoListaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public string Categoria { get; set; } = null!;
    }
}