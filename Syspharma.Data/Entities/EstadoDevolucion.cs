namespace Syspharma.Data.Entities;

public class EstadoDevolucion
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}