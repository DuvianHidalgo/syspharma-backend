using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Syspharma.API.Entities;

public partial class SyspharmaContext : IdentityDbContext<IdentityUser>
{
    public SyspharmaContext()
    {
    }

    public SyspharmaContext(DbContextOptions<SyspharmaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
    public virtual DbSet<Carrito> Carritos { get; set; }
    public virtual DbSet<CarritoDetalle> CarritoDetalles { get; set; }
    public virtual DbSet<Categoria> Categorias { get; set; }
    public virtual DbSet<Cita> Citas { get; set; }
    public virtual DbSet<Compra> Compras { get; set; }
    public virtual DbSet<CompraDetalle> CompraDetalles { get; set; }
    public virtual DbSet<Favorito> Favoritos { get; set; }
    public virtual DbSet<Gasto> Gastos { get; set; }
    public virtual DbSet<Medico> Medicos { get; set; }
    public virtual DbSet<Pedido> Pedidos { get; set; }
    public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }
    public virtual DbSet<Permiso> Permisos { get; set; }
    public virtual DbSet<Producto> Productos { get; set; }
    public virtual DbSet<Proveedore> Proveedores { get; set; }
    public new virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RolesPermiso> RolesPermisos { get; set; }
    public virtual DbSet<Servicio> Servicios { get; set; }
    public virtual DbSet<Turno> Turnos { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<VResumenTurno> VResumenTurnos { get; set; }
    public virtual DbSet<Venta> Ventas { get; set; }
    public virtual DbSet<VentaDetalle> VentaDetalles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-SANTIAG\\SQLEXPRESS;Database=syspharma;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");
            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex("RoleId", "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");
            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");
            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__carritos__3213E83F3310EC39");
            entity.ToTable("carritos");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasMaxLength(20).IsUnicode(false).HasDefaultValue("activo").HasColumnName("estado");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())").HasColumnName("fechaCreacion");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.HasOne(d => d.Usuario).WithMany(p => p.Carritos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carritos_Usuario");
        });

        modelBuilder.Entity<CarritoDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__carrito___3213E83FC2AB1E59");
            entity.ToTable("carrito_detalles");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CarritoId).HasColumnName("carritoId");
            entity.Property(e => e.ProductoId).HasColumnName("productoId");
            entity.HasOne(d => d.Carrito).WithMany(p => p.CarritoDetalles).HasForeignKey(d => d.CarritoId).HasConstraintName("FK_CarritoDetalles_Carrito");
            entity.HasOne(d => d.Producto).WithMany(p => p.CarritoDetalles).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CarritoDetalles_Producto");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__categori__3213E83F430E4B9B");
            entity.ToTable("categorias");
            entity.HasIndex(e => e.Nombre, "UQ__categori__72AFBCC684B02B18").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasDefaultValue(true).HasColumnName("estado");
            entity.Property(e => e.Nombre).HasMaxLength(100).HasColumnName("nombre");
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__citas__3213E83F65E79126");
            entity.ToTable("citas");
            entity.HasIndex(e => new { e.MedicoId, e.Fecha }, "idx_citas_medico_fecha").IsDescending(false, true);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasMaxLength(30).IsUnicode(false).HasDefaultValue("Confirmar Asistencia").HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())").HasColumnName("fechaCreacion");
            entity.Property(e => e.Hora).HasColumnName("hora");
            entity.Property(e => e.MedicoId).HasColumnName("medicoId");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.PacienteDocumento).HasMaxLength(20).IsUnicode(false).HasColumnName("pacienteDocumento");
            entity.Property(e => e.PacienteEmail).HasMaxLength(100).IsUnicode(false).HasColumnName("pacienteEmail");
            entity.Property(e => e.PacienteNombre).HasMaxLength(200).HasColumnName("pacienteNombre");
            entity.Property(e => e.PacienteTelefono).HasMaxLength(20).IsUnicode(false).HasColumnName("pacienteTelefono");
            entity.Property(e => e.Precio).HasColumnType("decimal(12, 2)").HasColumnName("precio");
            entity.Property(e => e.ServicioNombre).HasMaxLength(100).IsUnicode(false).HasColumnName("servicioNombre");
            entity.HasOne(d => d.Medico).WithMany(p => p.Cita).HasForeignKey(d => d.MedicoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Citas_Medicos");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__compras__3213E83F2E6C2C84");
            entity.ToTable("compras");
            entity.HasIndex(e => e.NumeroCompra, "UQ_Compras_Numero").IsUnique();
            entity.HasIndex(e => e.ProveedorId, "idx_compras_proveedor");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasMaxLength(20).IsUnicode(false).HasDefaultValue("registrada").HasColumnName("estado");
            entity.Property(e => e.FechaCompra).HasDefaultValueSql("(getdate())").HasColumnName("fechaCompra");
            entity.Property(e => e.FechaEntrega).HasColumnName("fechaEntrega");
            entity.Property(e => e.Impuesto).HasDefaultValue(0m).HasColumnType("decimal(12, 2)").HasColumnName("impuesto");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.NumeroCompra).HasMaxLength(50).IsUnicode(false).HasColumnName("numeroCompra");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedorId");
            entity.Property(e => e.Subtotal).HasDefaultValue(0m).HasColumnType("decimal(12, 2)").HasColumnName("subtotal");
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)").HasColumnName("total");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.HasOne(d => d.Proveedor).WithMany(p => p.Compras).HasForeignKey(d => d.ProveedorId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Compras_Proveedor");
            entity.HasOne(d => d.Usuario).WithMany(p => p.Compras).HasForeignKey(d => d.UsuarioId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Compras_Usuario");
        });

        modelBuilder.Entity<CompraDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__compra_d__3213E83F46C11802");
            entity.ToTable("compra_detalles");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CompraId).HasColumnName("compraId");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(12, 2)").HasColumnName("precioUnitario");
            entity.Property(e => e.ProductoId).HasColumnName("productoId");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(12, 2)").HasColumnName("subtotal");
            entity.HasOne(d => d.Compra).WithMany(p => p.CompraDetalles).HasForeignKey(d => d.CompraId).HasConstraintName("FK_CompraDetalles_Compra");
            entity.HasOne(d => d.Producto).WithMany(p => p.CompraDetalles).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CompraDetalles_Producto");
        });

        modelBuilder.Entity<Favorito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__favorito__3213E83F2BEBF947");
            entity.ToTable("favoritos");
            entity.HasIndex(e => new { e.UsuarioId, e.ProductoId }, "unique_usuario_producto").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaAgregado).HasDefaultValueSql("(getdate())").HasColumnName("fechaAgregado");
            entity.Property(e => e.ProductoId).HasColumnName("productoId");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.HasOne(d => d.Producto).WithMany(p => p.Favoritos).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Favoritos_Producto");
            entity.HasOne(d => d.Usuario).WithMany(p => p.Favoritos).HasForeignKey(d => d.UsuarioId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Favoritos_Usuario");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__gastos__3213E83F9DA3A73A");
            entity.ToTable("gastos");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria).HasMaxLength(20).IsUnicode(false).HasDefaultValue("operacional").HasColumnName("categoria");
            entity.Property(e => e.Comprobante).HasMaxLength(100).IsUnicode(false).HasColumnName("comprobante");
            entity.Property(e => e.Concepto).HasMaxLength(200).HasColumnName("concepto");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaGasto).HasDefaultValueSql("(getdate())").HasColumnName("fechaGasto");
            entity.Property(e => e.Monto).HasColumnType("decimal(12, 2)").HasColumnName("monto");
            entity.Property(e => e.TurnoId).HasColumnName("turnoId");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.HasOne(d => d.Turno).WithMany(p => p.Gastos).HasForeignKey(d => d.TurnoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Gastos_Turnos");
            entity.HasOne(d => d.Usuario).WithMany(p => p.Gastos).HasForeignKey(d => d.UsuarioId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Gastos_Usuarios");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__medicos__3213E83F72005C98");
            entity.ToTable("medicos");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiasLaborales).HasMaxLength(50).IsUnicode(false).HasColumnName("diasLaborales");
            entity.Property(e => e.Documento).HasMaxLength(20).IsUnicode(false).HasColumnName("documento");
            entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false).HasColumnName("email");
            entity.Property(e => e.Especialidad).HasMaxLength(100).HasColumnName("especialidad");
            entity.Property(e => e.Estado).HasDefaultValue(true).HasColumnName("estado");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())").HasColumnName("fechaCreacion");
            entity.Property(e => e.HoraFin).HasColumnName("horaFin");
            entity.Property(e => e.HoraInicio).HasColumnName("horaInicio");
            entity.Property(e => e.Intervalo).HasDefaultValue(30).HasColumnName("intervalo");
            entity.Property(e => e.Nombre).HasMaxLength(150).HasColumnName("nombre");
            entity.Property(e => e.Telefono).HasMaxLength(20).IsUnicode(false).HasColumnName("telefono");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pedidos__3213E83FD48AD172");
            entity.ToTable("pedidos");
            entity.HasIndex(e => e.NumeroPedido, "UQ__pedidos__90DD614969B219F9").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("clienteId");
            entity.Property(e => e.EmpleadoId).HasColumnName("empleadoId");
            entity.Property(e => e.Estado).HasMaxLength(20).IsUnicode(false).HasDefaultValue("pendiente").HasColumnName("estado");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())").HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaEntrega).HasColumnName("fechaEntrega");
            entity.Property(e => e.NumeroPedido).HasMaxLength(50).IsUnicode(false).HasColumnName("numeroPedido");
            entity.Property(e => e.Origen).HasMaxLength(20).IsUnicode(false).HasColumnName("origen");
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)").HasColumnName("total");
            entity.HasOne(d => d.Cliente).WithMany(p => p.PedidoClientes).HasForeignKey(d => d.ClienteId).HasConstraintName("FK_Pedidos_Cliente");
            entity.HasOne(d => d.Empleado).WithMany(p => p.PedidoEmpleados).HasForeignKey(d => d.EmpleadoId).HasConstraintName("FK_Pedidos_Empleado");
        });

        modelBuilder.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pedido_d__3213E83FBB424CE8");
            entity.ToTable("pedido_detalles");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.PedidoId).HasColumnName("pedidoId");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(12, 2)").HasColumnName("precioUnitario");
            entity.Property(e => e.ProductoId).HasColumnName("productoId");
            entity.Property(e => e.ServicioId).HasColumnName("servicioId");
            entity.Property(e => e.Tipo).HasMaxLength(20).IsUnicode(false).HasColumnName("tipo");
            entity.HasOne(d => d.Pedido).WithMany(p => p.PedidoDetalles).HasForeignKey(d => d.PedidoId).HasConstraintName("FK_PedidoDetalles_Pedido");
            entity.HasOne(d => d.Producto).WithMany(p => p.PedidoDetalles).HasForeignKey(d => d.ProductoId).HasConstraintName("FK_PedidoDetalles_Producto");
            entity.HasOne(d => d.Servicio).WithMany(p => p.PedidoDetalles).HasForeignKey(d => d.ServicioId).HasConstraintName("FK_PedidoDetalles_Servicio");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__permisos__3213E83FD74BF38A");
            entity.ToTable("permisos");
            entity.HasIndex(e => e.Codigo, "UQ__permisos__40F9A206807E2333").IsUnique();
            entity.HasIndex(e => e.Categoria, "idx_permisos_categoria");
            entity.HasIndex(e => e.Codigo, "idx_permisos_codigo");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria).HasMaxLength(50).IsUnicode(false).HasColumnName("categoria");
            entity.Property(e => e.Codigo).HasMaxLength(50).IsUnicode(false).HasColumnName("codigo");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())").HasColumnName("fechaCreacion");
            entity.Property(e => e.Nombre).HasMaxLength(100).HasColumnName("nombre");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__producto__3213E83FA9923A2B");
            entity.ToTable("productos");
            entity.HasIndex(e => e.Sku, "UQ__producto__DDDF4BE7DEDA9D35").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoriaId");
            entity.Property(e => e.CodigoBarras).HasMaxLength(100).IsUnicode(false).HasColumnName("codigoBarras");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasDefaultValue(true).HasColumnName("estado");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())").HasColumnName("fechaCreacion");
            entity.Property(e => e.Imagen).HasColumnName("imagen");
            entity.Property(e => e.Nombre).HasMaxLength(200).HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnType("decimal(12, 2)").HasColumnName("precio");
            entity.Property(e => e.PrecioCompra).HasColumnType("decimal(12, 2)").HasColumnName("precioCompra");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedorId");
            entity.Property(e => e.Sku).HasMaxLength(50).IsUnicode(false).HasColumnName("sku");
            entity.Property(e => e.Stock).HasDefaultValue(0).HasColumnName("stock");
            entity.Property(e => e.UltimaActualizacion).HasDefaultValueSql("(getdate())").HasColumnName("ultimaActualizacion");
            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos).HasForeignKey(d => d.CategoriaId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Productos_Categorias");
            entity.HasOne(d => d.Proveedor).WithMany(p => p.Productos).HasForeignKey(d => d.ProveedorId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_Productos_Proveedores");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__proveedo__3213E83F7D8E1645");
            entity.ToTable("proveedores");
            entity.HasIndex(e => e.Nombre, "UQ__proveedo__72AFBCC694CE12AD").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contacto).HasMaxLength(150).HasColumnName("contacto");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false).HasColumnName("email");
            entity.Property(e => e.Estado).HasDefaultValue(true).HasColumnName("estado");
            entity.Property(e => e.Nombre).HasMaxLength(150).HasColumnName("nombre");
            entity.Property(e => e.Telefono).HasMaxLength(20).IsUnicode(false).HasColumnName("telefono");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83FE520A6C5");
            entity.ToTable("roles");
            entity.HasIndex(e => e.Nombre, "UQ__roles__72AFBCC6789C1F2F").IsUnique();
            entity.HasIndex(e => e.Estado, "idx_roles_estado");
            entity.HasIndex(e => e.Nombre, "idx_roles_nombre");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasDefaultValue(true).HasColumnName("estado");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())").HasColumnName("fechaCreacion");
            entity.Property(e => e.Nombre).HasMaxLength(50).IsUnicode(false).HasColumnName("nombre");
        });

        modelBuilder.Entity<RolesPermiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles_pe__3213E83F0EE21B98");
            entity.ToTable("roles_permisos");
            entity.HasIndex(e => e.PermisoId, "idx_rolespermisos_permisoId");
            entity.HasIndex(e => e.RoleId, "idx_rolespermisos_roleId");
            entity.HasIndex(e => new { e.RoleId, e.PermisoId }, "unique_role_permiso").IsUnique();
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaAsignacion).HasDefaultValueSql("(getdate())").HasColumnName("fechaAsignacion");
            entity.Property(e => e.PermisoId).HasColumnName("permisoId");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.HasOne(d => d.Permiso).WithMany(p => p.RolesPermisos).HasForeignKey(d => d.PermisoId).HasConstraintName("FK_RolesPermisos_Permisos");
            entity.HasOne(d => d.Role).WithMany(p => p.RolesPermisos).HasForeignKey(d => d.RoleId).HasConstraintName("FK_RolesPermisos_Roles");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__servicio__3213E83FEC352FDF");
            entity.ToTable("servicios");
            entity.HasIndex(e => new { e.MedicoId, e.FechaServicio }, "idx_servicios_medico_fecha").IsDescending(false, true);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CitaId).HasColumnName("citaId");
            entity.Property(e => e.FechaServicio).HasDefaultValueSql("(getdate())").HasColumnName("fechaServicio");
            entity.Property(e => e.MedicoId).HasColumnName("medicoId");
            entity.Property(e => e.MedicoNombre).HasMaxLength(150).IsUnicode(false).HasColumnName("medicoNombre");
            entity.Property(e => e.Monto).HasColumnType("decimal(12, 2)").HasColumnName("monto");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.PacienteDocumento).HasMaxLength(20).IsUnicode(false).HasColumnName("pacienteDocumento");
            entity.Property(e => e.PacienteNombre).HasMaxLength(200).HasColumnName("pacienteNombre");
            entity.Property(e => e.ServicioNombre).HasMaxLength(100).IsUnicode(false).HasColumnName("servicioNombre");
            entity.Property(e => e.TurnoId).HasColumnName("turnoId");
            entity.HasOne(d => d.Cita).WithMany(p => p.Servicios).HasForeignKey(d => d.CitaId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_Servicios_Citas");
            entity.HasOne(d => d.Medico).WithMany(p => p.Servicios).HasForeignKey(d => d.MedicoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Servicios_Medicos");
            entity.HasOne(d => d.Turno).WithMany(p => p.Servicios).HasForeignKey(d => d.TurnoId).HasConstraintName("FK_Servicios_Turnos");
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__turnos__3213E83FDE8CCB64");
            entity.ToTable("turnos");
            entity.HasIndex(e => new { e.Estado, e.FechaApertura }, "idx_turnos_estado_fecha").IsDescending(false, true);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Diferencia).HasColumnType("decimal(12, 2)").HasColumnName("diferencia");
            entity.Property(e => e.Estado).HasMaxLength(20).IsUnicode(false).HasDefaultValue("activo").HasColumnName("estado");
            entity.Property(e => e.FechaApertura).HasDefaultValueSql("(getdate())").HasColumnName("fechaApertura");
            entity.Property(e => e.FechaCierre).HasColumnName("fechaCierre");
            entity.Property(e => e.MontoBase).HasColumnType("decimal(12, 2)").HasColumnName("montoBase");
            entity.Property(e => e.MontoFinal).HasColumnType("decimal(12, 2)").HasColumnName("montoFinal");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.ResumenErroresCaja).HasDefaultValue(0m).HasColumnType("decimal(12, 2)").HasColumnName("resumenErroresCaja");
            entity.Property(e => e.ResumenServicios).HasDefaultValue(0).HasColumnName("resumenServicios");
            entity.Property(e => e.ResumenVentas).HasDefaultValue(0).HasColumnName("resumenVentas");
            entity.Property(e => e.TotalGastos).HasDefaultValue(0m).HasColumnType("decimal(12, 2)").HasColumnName("totalGastos");
            entity.Property(e => e.TotalVentas).HasDefaultValue(0m).HasColumnType("decimal(12, 2)").HasColumnName("totalVentas");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.HasOne(d => d.Usuario).WithMany(p => p.Turnos).HasForeignKey(d => d.UsuarioId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Turnos_Usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuarios__3213E83F278FD5C8");
            entity.ToTable("usuarios");
            entity.HasIndex(e => e.Documento, "UQ__usuarios__A25B3E61A900D650").IsUnique();
            entity.HasIndex(e => e.Email, "UQ__usuarios__AB6E616461D1B7DB").IsUnique();
            entity.HasIndex(e => e.Email, "idx_usuarios_email");
            entity.HasIndex(e => e.RoleId, "idx_usuarios_roleId");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Avatar).HasColumnName("avatar");
            entity.Property(e => e.Documento).HasMaxLength(20).IsUnicode(false).HasColumnName("documento");
            entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false).HasColumnName("email");
            entity.Property(e => e.Estado).HasDefaultValue(true).HasColumnName("estado");
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())").HasColumnName("fechaCreacion");
            entity.Property(e => e.Nombre).HasMaxLength(150).HasColumnName("nombre");
            entity.Property(e => e.Password).HasMaxLength(255).IsUnicode(false).HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.Telefono).HasMaxLength(20).IsUnicode(false).HasColumnName("telefono");
            entity.Property(e => e.TipoDocumento).HasMaxLength(20).IsUnicode(false).HasColumnName("tipoDocumento");
            entity.Property(e => e.UltimoAcceso).HasColumnName("ultimoAcceso");
            entity.HasOne(d => d.Role).WithMany(p => p.Usuarios).HasForeignKey(d => d.RoleId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Usuarios_Roles");
        });

        modelBuilder.Entity<VResumenTurno>(entity =>
        {
            entity.HasNoKey().ToView("v_resumen_turnos");
            entity.Property(e => e.Empleado).HasMaxLength(150).HasColumnName("empleado");
            entity.Property(e => e.Estado).HasMaxLength(20).IsUnicode(false).HasColumnName("estado");
            entity.Property(e => e.FechaApertura).HasColumnName("fechaApertura");
            entity.Property(e => e.FechaCierre).HasColumnName("fechaCierre");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MontoBase).HasColumnType("decimal(12, 2)").HasColumnName("montoBase");
            entity.Property(e => e.TotalGastos).HasColumnType("decimal(38, 2)").HasColumnName("totalGastos");
            entity.Property(e => e.TotalVentas).HasColumnType("decimal(38, 2)").HasColumnName("totalVentas");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ventas__3213E83F2F14C49F");
            entity.ToTable("ventas");
            entity.HasIndex(e => e.NumeroVenta, "UQ__ventas__44FDAC49F683221F").IsUnique();
            entity.HasIndex(e => new { e.TurnoId, e.FechaVenta }, "idx_ventas_turno_fecha").IsDescending(false, true);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteDocumento).HasMaxLength(20).IsUnicode(false).HasColumnName("clienteDocumento");
            entity.Property(e => e.ClienteNombre).HasMaxLength(150).IsUnicode(false).HasColumnName("clienteNombre");
            entity.Property(e => e.ClienteTelefono).HasMaxLength(20).IsUnicode(false).HasColumnName("clienteTelefono");
            entity.Property(e => e.Estado).HasMaxLength(20).IsUnicode(false).HasDefaultValue("completada").HasColumnName("estado");
            entity.Property(e => e.FechaVenta).HasDefaultValueSql("(getdate())").HasColumnName("fechaVenta");
            entity.Property(e => e.Impuesto).HasDefaultValue(0m).HasColumnType("decimal(12, 2)").HasColumnName("impuesto");
            entity.Property(e => e.MetodoPago).HasMaxLength(20).IsUnicode(false).HasDefaultValue("efectivo").HasColumnName("metodoPago");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.NumeroVenta).HasMaxLength(50).IsUnicode(false).HasColumnName("numeroVenta");
            entity.Property(e => e.Subtotal).HasDefaultValue(0m).HasColumnType("decimal(12, 2)").HasColumnName("subtotal");
            entity.Property(e => e.Total).HasColumnType("decimal(12, 2)").HasColumnName("total");
            entity.Property(e => e.TurnoId).HasColumnName("turnoId");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.HasOne(d => d.Turno).WithMany(p => p.Venta).HasForeignKey(d => d.TurnoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Ventas_Turnos");
            entity.HasOne(d => d.Usuario).WithMany(p => p.Venta).HasForeignKey(d => d.UsuarioId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Ventas_Usuarios");
        });

        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__venta_de__3213E83F5B73A172");
            entity.ToTable("venta_detalles");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Descuento).HasDefaultValue(0m).HasColumnType("decimal(12, 2)").HasColumnName("descuento");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(12, 2)").HasColumnName("precioUnitario");
            entity.Property(e => e.ProductoId).HasColumnName("productoId");
            entity.Property(e => e.VentaId).HasColumnName("ventaId");
            entity.HasOne(d => d.Producto).WithMany(p => p.VentaDetalles).HasForeignKey(d => d.ProductoId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Detalles_Productos");
            entity.HasOne(d => d.Venta).WithMany(p => p.VentaDetalles).HasForeignKey(d => d.VentaId).HasConstraintName("FK_Detalles_Ventas");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}