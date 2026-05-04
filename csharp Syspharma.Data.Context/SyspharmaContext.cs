using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Syspharma.Data.Entities;

public partial class SyspharmaContext : IdentityDbContext<ApplicationUser>
{
    // conserva el resto del contenido del DbContext tal cual
}