using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Entities;

public partial class SyspharmaContext : IdentityDbContext<Usuario>
{
    // conserva el resto del contenido del DbContext tal cual
}