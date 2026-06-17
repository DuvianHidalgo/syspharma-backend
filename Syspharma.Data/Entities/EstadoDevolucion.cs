<<<<<<< Updated upstream
﻿namespace Syspharma.Data.Entities;
=======
namespace Syspharma.Data.Entities;
>>>>>>> Stashed changes

public class EstadoDevolucion
{
    public int Id { get; set; }
<<<<<<< Updated upstream
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
=======
    public string Nombre { get; set; } = null!;
    public bool Activo { get; set; }
    public virtual ICollection<Devolucion> Devoluciones { get; set; } = new List<Devolucion>();
}
>>>>>>> Stashed changes
