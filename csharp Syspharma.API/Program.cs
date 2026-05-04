// añadir using:
// using Microsoft.AspNetCore.Identity;
// using Syspharma.Data.Entities;

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Opciones básicas (ajusta según tus necesidades)
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<SyspharmaContext>()
.AddDefaultTokenProviders();