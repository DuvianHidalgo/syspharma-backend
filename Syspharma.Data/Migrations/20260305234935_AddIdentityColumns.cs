using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Syspharma.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__categori__3213E83F9886B4BE", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "medicos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    especialidad = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    diasLaborales = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    horaInicio = table.Column<TimeOnly>(type: "time", nullable: true),
                    horaFin = table.Column<TimeOnly>(type: "time", nullable: true),
                    intervalo = table.Column<int>(type: "int", nullable: true, defaultValue: 30),
                    estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__medicos__3213E83F259FD362", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permisos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    categoria = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__permisos__3213E83F1E28B840", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "proveedores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    contacto = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ciudad = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__proveedo__3213E83FF3647DE3", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles__3213E83F51C39779", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "citas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    medicoId = table.Column<int>(type: "int", nullable: false),
                    pacienteNombre = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    pacienteDocumento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    pacienteTelefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    pacienteEmail = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    hora = table.Column<TimeOnly>(type: "time", nullable: false),
                    servicioNombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    precio = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    estado = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true, defaultValue: "Confirmar Asistencia"),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__citas__3213E83F6870EB6E", x => x.id);
                    table.ForeignKey(
                        name: "FK_Citas_Medicos",
                        column: x => x.medicoId,
                        principalTable: "medicos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    categoriaId = table.Column<int>(type: "int", nullable: false),
                    proveedorId = table.Column<int>(type: "int", nullable: true),
                    precio = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    precioCompra = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    stock = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    sku = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    codigoBarras = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    imagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    ultimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__producto__3213E83F8AE3CB5C", x => x.id);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias",
                        column: x => x.categoriaId,
                        principalTable: "categorias",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Productos_Proveedores",
                        column: x => x.proveedorId,
                        principalTable: "proveedores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "roles_permisos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    permisoId = table.Column<int>(type: "int", nullable: false),
                    fechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles_pe__3213E83FA50D9FAE", x => x.id);
                    table.ForeignKey(
                        name: "FK_RolesPermisos_Permisos",
                        column: x => x.permisoId,
                        principalTable: "permisos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesPermisos_Roles",
                        column: x => x.roleId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    documento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    tipoDocumento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    fechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    ultimoAcceso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles",
                        column: x => x.roleId,
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "compras",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeroCompra = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    proveedorId = table.Column<int>(type: "int", nullable: false),
                    usuarioId = table.Column<int>(type: "int", nullable: false),
                    fechaCompra = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    fechaEntrega = table.Column<DateOnly>(type: "date", nullable: true),
                    subtotal = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    impuesto = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    total = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    estado = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "pendiente"),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__compras__3213E83FDED3A8CE", x => x.id);
                    table.ForeignKey(
                        name: "FK_Compras_Proveedores",
                        column: x => x.proveedorId,
                        principalTable: "proveedores",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Compras_Usuarios",
                        column: x => x.usuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "turnos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioId = table.Column<int>(type: "int", nullable: false),
                    fechaApertura = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    fechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    montoBase = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    totalVentas = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    totalGastos = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    montoFinal = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    diferencia = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    estado = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "activo"),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resumenVentas = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    resumenServicios = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    resumenErroresCaja = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__turnos__3213E83FB24084EB", x => x.id);
                    table.ForeignKey(
                        name: "FK_Turnos_Usuarios",
                        column: x => x.usuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "compra_detalles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    compraId = table.Column<int>(type: "int", nullable: false),
                    productoId = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precioUnitario = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__compra_d__3213E83F9B4482A4", x => x.id);
                    table.ForeignKey(
                        name: "FK_CDetalles_Compras",
                        column: x => x.compraId,
                        principalTable: "compras",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CDetalles_Productos",
                        column: x => x.productoId,
                        principalTable: "productos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "gastos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    turnoId = table.Column<int>(type: "int", nullable: false),
                    usuarioId = table.Column<int>(type: "int", nullable: false),
                    concepto = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    monto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    categoria = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "operacional"),
                    fechaGasto = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comprobante = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__gastos__3213E83F56FE0EDF", x => x.id);
                    table.ForeignKey(
                        name: "FK_Gastos_Turnos",
                        column: x => x.turnoId,
                        principalTable: "turnos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Gastos_Usuarios",
                        column: x => x.usuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "servicios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    citaId = table.Column<int>(type: "int", nullable: true),
                    medicoId = table.Column<int>(type: "int", nullable: false),
                    medicoNombre = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    pacienteNombre = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    pacienteDocumento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    servicioNombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    monto = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    turnoId = table.Column<int>(type: "int", nullable: true),
                    fechaServicio = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__servicio__3213E83FB57629D7", x => x.id);
                    table.ForeignKey(
                        name: "FK_Servicios_Citas",
                        column: x => x.citaId,
                        principalTable: "citas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Servicios_Medicos",
                        column: x => x.medicoId,
                        principalTable: "medicos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Servicios_Turnos",
                        column: x => x.turnoId,
                        principalTable: "turnos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ventas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeroVenta = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    turnoId = table.Column<int>(type: "int", nullable: false),
                    usuarioId = table.Column<int>(type: "int", nullable: false),
                    clienteNombre = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    clienteDocumento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    clienteTelefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    fechaVenta = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    subtotal = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    impuesto = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m),
                    total = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    metodoPago = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "efectivo"),
                    estado = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "completada"),
                    notas = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ventas__3213E83FF7A5F5E7", x => x.id);
                    table.ForeignKey(
                        name: "FK_Ventas_Turnos",
                        column: x => x.turnoId,
                        principalTable: "turnos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Ventas_Usuarios",
                        column: x => x.usuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "venta_detalles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ventaId = table.Column<int>(type: "int", nullable: false),
                    productoId = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precioUnitario = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    descuento = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__venta_de__3213E83FE252B299", x => x.id);
                    table.ForeignKey(
                        name: "FK_Detalles_Productos",
                        column: x => x.productoId,
                        principalTable: "productos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Detalles_Ventas",
                        column: x => x.ventaId,
                        principalTable: "ventas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__categori__72AFBCC6A00018BA",
                table: "categorias",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_citas_medico_fecha",
                table: "citas",
                columns: new[] { "medicoId", "fecha" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_compra_detalles_compraId",
                table: "compra_detalles",
                column: "compraId");

            migrationBuilder.CreateIndex(
                name: "IX_compra_detalles_productoId",
                table: "compra_detalles",
                column: "productoId");

            migrationBuilder.CreateIndex(
                name: "idx_compras_proveedor",
                table: "compras",
                column: "proveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_compras_usuarioId",
                table: "compras",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "UQ__compras__6EB8ED5122FC55F7",
                table: "compras",
                column: "numeroCompra",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_gastos_turnoId",
                table: "gastos",
                column: "turnoId");

            migrationBuilder.CreateIndex(
                name: "IX_gastos_usuarioId",
                table: "gastos",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "UQ__permisos__40F9A206CB3EF4FA",
                table: "permisos",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_productos_categoria_estado",
                table: "productos",
                columns: new[] { "categoriaId", "estado" });

            migrationBuilder.CreateIndex(
                name: "idx_productos_nombre",
                table: "productos",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "IX_productos_proveedorId",
                table: "productos",
                column: "proveedorId");

            migrationBuilder.CreateIndex(
                name: "UQ__producto__DDDF4BE705E56FDA",
                table: "productos",
                column: "sku",
                unique: true,
                filter: "[sku] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__proveedo__72AFBCC6A995FAF4",
                table: "proveedores",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__roles__72AFBCC6521A905E",
                table: "roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_permisos_permisoId",
                table: "roles_permisos",
                column: "permisoId");

            migrationBuilder.CreateIndex(
                name: "UQ_Role_Permiso",
                table: "roles_permisos",
                columns: new[] { "roleId", "permisoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_servicios_medico_fecha",
                table: "servicios",
                columns: new[] { "medicoId", "fechaServicio" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_servicios_citaId",
                table: "servicios",
                column: "citaId");

            migrationBuilder.CreateIndex(
                name: "IX_servicios_turnoId",
                table: "servicios",
                column: "turnoId");

            migrationBuilder.CreateIndex(
                name: "idx_turnos_estado_fecha",
                table: "turnos",
                columns: new[] { "estado", "fechaApertura" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_turnos_usuarioId",
                table: "turnos",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "usuarios",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_roleId",
                table: "usuarios",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "usuarios",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_venta_detalles_productoId",
                table: "venta_detalles",
                column: "productoId");

            migrationBuilder.CreateIndex(
                name: "IX_venta_detalles_ventaId",
                table: "venta_detalles",
                column: "ventaId");

            migrationBuilder.CreateIndex(
                name: "idx_ventas_turno_fecha",
                table: "ventas",
                columns: new[] { "turnoId", "fechaVenta" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_ventas_usuarioId",
                table: "ventas",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "UQ__ventas__44FDAC49AB629988",
                table: "ventas",
                column: "numeroVenta",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "compra_detalles");

            migrationBuilder.DropTable(
                name: "gastos");

            migrationBuilder.DropTable(
                name: "roles_permisos");

            migrationBuilder.DropTable(
                name: "servicios");

            migrationBuilder.DropTable(
                name: "venta_detalles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "compras");

            migrationBuilder.DropTable(
                name: "permisos");

            migrationBuilder.DropTable(
                name: "citas");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "ventas");

            migrationBuilder.DropTable(
                name: "medicos");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "proveedores");

            migrationBuilder.DropTable(
                name: "turnos");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
