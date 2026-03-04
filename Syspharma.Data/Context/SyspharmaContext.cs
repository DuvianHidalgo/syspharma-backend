using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Syspharma.Data.Entities;

namespace Syspharma.Data.Context;

public partial class SyspharmaContext : DbContext
{
    public SyspharmaContext()
    {
    }

    public SyspharmaContext(DbContextOptions<SyspharmaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<CompraDetalle> CompraDetalles { get; set; }

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolesPermiso> RolesPermisos { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<Turno> Turnos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VCompraDetalle> VCompraDetalles { get; set; }

    public virtual DbSet<VResumenTurno> VResumenTurnos { get; set; }

    public virtual DbSet<VVentaDetalle> VVentaDetalles { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<VentaDetalle> VentaDetalles { get; set; }

<<<<<<< HEAD
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
=======
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-5BJJQ8K\\SQLEXPRESS;Database=syspharma;Trusted_Connection=True;TrustServerCertificate=True");
>>>>>>> 0cf9820041e58f3591f56cc28d0617f8b91ba551

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__categori__3213E83F9886B4BE");

            entity.ToTable("categorias");

            entity.HasIndex(e => e.Nombre, "UQ__categori__72AFBCC6A00018BA").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__citas__3213E83F6870EB6E");

            entity.ToTable("citas");

            entity.HasIndex(e => new { e.MedicoId, e.Fecha }, "idx_citas_medico_fecha").IsDescending(false, true);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Confirmar Asistencia")
                .HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.Hora).HasColumnName("hora");
            entity.Property(e => e.MedicoId).HasColumnName("medicoId");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.PacienteDocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("pacienteDocumento");
            entity.Property(e => e.PacienteEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pacienteEmail");
            entity.Property(e => e.PacienteNombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pacienteNombre");
            entity.Property(e => e.PacienteTelefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("pacienteTelefono");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.ServicioNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("servicioNombre");

            entity.HasOne(d => d.Medico).WithMany(p => p.Cita)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Medicos");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__compras__3213E83FDED3A8CE");

            entity.ToTable("compras");

            entity.HasIndex(e => e.NumeroCompra, "UQ__compras__6EB8ED5122FC55F7").IsUnique();

            entity.HasIndex(e => e.ProveedorId, "idx_compras_proveedor");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("pendiente")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCompra)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCompra");
            entity.Property(e => e.FechaEntrega).HasColumnName("fechaEntrega");
            entity.Property(e => e.Impuesto)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("impuesto");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.NumeroCompra)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroCompra");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedorId");
            entity.Property(e => e.Subtotal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Compras)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compras_Proveedores");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Compras)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Compras_Usuarios");
        });

        modelBuilder.Entity<CompraDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__compra_d__3213E83F9B4482A4");

            entity.ToTable("compra_detalles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CompraId).HasColumnName("compraId");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precioUnitario");
            entity.Property(e => e.ProductoId).HasColumnName("productoId");

            entity.HasOne(d => d.Compra).WithMany(p => p.CompraDetalles)
                .HasForeignKey(d => d.CompraId)
                .HasConstraintName("FK_CDetalles_Compras");

            entity.HasOne(d => d.Producto).WithMany(p => p.CompraDetalles)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CDetalles_Productos");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__gastos__3213E83F56FE0EDF");

            entity.ToTable("gastos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("operacional")
                .HasColumnName("categoria");
            entity.Property(e => e.Comprobante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("comprobante");
            entity.Property(e => e.Concepto)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("concepto");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaGasto)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaGasto");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.TurnoId).HasColumnName("turnoId");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Turno).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.TurnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gastos_Turnos");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gastos_Usuarios");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__medicos__3213E83F259FD362");

            entity.ToTable("medicos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiasLaborales)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("diasLaborales");
            entity.Property(e => e.Documento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("documento");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Especialidad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("especialidad");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.HoraFin).HasColumnName("horaFin");
            entity.Property(e => e.HoraInicio).HasColumnName("horaInicio");
            entity.Property(e => e.Intervalo)
                .HasDefaultValue(30)
                .HasColumnName("intervalo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__permisos__3213E83F1E28B840");

            entity.ToTable("permisos");

            entity.HasIndex(e => e.Codigo, "UQ__permisos__40F9A206CB3EF4FA").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("categoria");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigo");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__producto__3213E83F8AE3CB5C");

            entity.ToTable("productos");

            entity.HasIndex(e => e.Sku, "UQ__producto__DDDF4BE705E56FDA").IsUnique();

            entity.HasIndex(e => new { e.CategoriaId, e.Estado }, "idx_productos_categoria_estado");

            entity.HasIndex(e => e.Nombre, "idx_productos_nombre");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoriaId");
            entity.Property(e => e.CodigoBarras)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("codigoBarras");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.Imagen).HasColumnName("imagen");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.PrecioCompra)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precioCompra");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedorId");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sku");
            entity.Property(e => e.Stock)
                .HasDefaultValue(0)
                .HasColumnName("stock");
            entity.Property(e => e.UltimaActualizacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ultimaActualizacion");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Categorias");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Productos)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Productos_Proveedores");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__proveedo__3213E83FF3647DE3");

            entity.ToTable("proveedores");

            entity.HasIndex(e => e.Nombre, "UQ__proveedo__72AFBCC6A995FAF4").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ciudad");
            entity.Property(e => e.Contacto)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("contacto");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F51C39779");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "UQ__roles__72AFBCC6521A905E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<RolesPermiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles_pe__3213E83FA50D9FAE");

            entity.ToTable("roles_permisos");

            entity.HasIndex(e => new { e.RoleId, e.PermisoId }, "UQ_Role_Permiso").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaAsignacion");
            entity.Property(e => e.PermisoId).HasColumnName("permisoId");
            entity.Property(e => e.RoleId).HasColumnName("roleId");

            entity.HasOne(d => d.Permiso).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.PermisoId)
                .HasConstraintName("FK_RolesPermisos_Permisos");

            entity.HasOne(d => d.Role).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_RolesPermisos_Roles");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__servicio__3213E83FB57629D7");

            entity.ToTable("servicios");

            entity.HasIndex(e => new { e.MedicoId, e.FechaServicio }, "idx_servicios_medico_fecha").IsDescending(false, true);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CitaId).HasColumnName("citaId");
            entity.Property(e => e.FechaServicio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaServicio");
            entity.Property(e => e.MedicoId).HasColumnName("medicoId");
            entity.Property(e => e.MedicoNombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("medicoNombre");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.PacienteDocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("pacienteDocumento");
            entity.Property(e => e.PacienteNombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pacienteNombre");
            entity.Property(e => e.ServicioNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("servicioNombre");
            entity.Property(e => e.TurnoId).HasColumnName("turnoId");

            entity.HasOne(d => d.Cita).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.CitaId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Servicios_Citas");

            entity.HasOne(d => d.Medico).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicios_Medicos");

            entity.HasOne(d => d.Turno).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.TurnoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Servicios_Turnos");
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__turnos__3213E83FB24084EB");

            entity.ToTable("turnos");

            entity.HasIndex(e => new { e.Estado, e.FechaApertura }, "idx_turnos_estado_fecha").IsDescending(false, true);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Diferencia)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("diferencia");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("activo")
                .HasColumnName("estado");
            entity.Property(e => e.FechaApertura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaApertura");
            entity.Property(e => e.FechaCierre).HasColumnName("fechaCierre");
            entity.Property(e => e.MontoBase)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("montoBase");
            entity.Property(e => e.MontoFinal)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("montoFinal");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.ResumenErroresCaja)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("resumenErroresCaja");
            entity.Property(e => e.ResumenServicios)
                .HasDefaultValue(0)
                .HasColumnName("resumenServicios");
            entity.Property(e => e.ResumenVentas)
                .HasDefaultValue(0)
                .HasColumnName("resumenVentas");
            entity.Property(e => e.TotalGastos)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("totalGastos");
            entity.Property(e => e.TotalVentas)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("totalVentas");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Turnos_Usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuarios__3213E83F63D932E8");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Documento, "UQ__usuarios__A25B3E610866FD8A").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__usuarios__AB6E61648B7D3B4E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Avatar).HasColumnName("avatar");
            entity.Property(e => e.Documento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("documento");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipoDocumento");
            entity.Property(e => e.UltimoAcceso).HasColumnName("ultimoAcceso");

            entity.HasOne(d => d.Role).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        modelBuilder.Entity<VCompraDetalle>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_compra_detalles");

            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CompraId).HasColumnName("compraId");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precioUnitario");
            entity.Property(e => e.ProductoId).HasColumnName("productoId");
            entity.Property(e => e.SubtotalCalculado)
                .HasColumnType("decimal(23, 2)")
                .HasColumnName("subtotal_calculado");
        });

        modelBuilder.Entity<VResumenTurno>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_resumen_turnos");

            entity.Property(e => e.Empleado)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("empleado");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaApertura).HasColumnName("fechaApertura");
            entity.Property(e => e.FechaCierre).HasColumnName("fechaCierre");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MontoBase)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("montoBase");
            entity.Property(e => e.TotalGastos)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("totalGastos");
            entity.Property(e => e.TotalVentas)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("totalVentas");
        });

        modelBuilder.Entity<VVentaDetalle>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_venta_detalles");

            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Descuento)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("descuento");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precioUnitario");
            entity.Property(e => e.ProductoId).HasColumnName("productoId");
            entity.Property(e => e.SubtotalCalculado)
                .HasColumnType("decimal(23, 2)")
                .HasColumnName("subtotal_calculado");
            entity.Property(e => e.VentaId).HasColumnName("ventaId");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ventas__3213E83FF7A5F5E7");

            entity.ToTable("ventas");

            entity.HasIndex(e => e.NumeroVenta, "UQ__ventas__44FDAC49AB629988").IsUnique();

            entity.HasIndex(e => new { e.TurnoId, e.FechaVenta }, "idx_ventas_turno_fecha").IsDescending(false, true);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteDocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("clienteDocumento");
            entity.Property(e => e.ClienteNombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("clienteNombre");
            entity.Property(e => e.ClienteTelefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("clienteTelefono");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("completada")
                .HasColumnName("estado");
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fechaVenta");
            entity.Property(e => e.Impuesto)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("impuesto");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("efectivo")
                .HasColumnName("metodoPago");
            entity.Property(e => e.Notas).HasColumnName("notas");
            entity.Property(e => e.NumeroVenta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroVenta");
            entity.Property(e => e.Subtotal)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total");
            entity.Property(e => e.TurnoId).HasColumnName("turnoId");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Turno).WithMany(p => p.Venta)
                .HasForeignKey(d => d.TurnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ventas_Turnos");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Venta)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ventas_Usuarios");
        });

        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__venta_de__3213E83FE252B299");

            entity.ToTable("venta_detalles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Descuento)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("descuento");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("precioUnitario");
            entity.Property(e => e.ProductoId).HasColumnName("productoId");
            entity.Property(e => e.VentaId).HasColumnName("ventaId");

            entity.HasOne(d => d.Producto).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detalles_Productos");

            entity.HasOne(d => d.Venta).WithMany(p => p.VentaDetalles)
                .HasForeignKey(d => d.VentaId)
                .HasConstraintName("FK_Detalles_Ventas");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
