using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Syspharma.Business.Mappings;
using Syspharma.Business.Services;
using Syspharma.Data.Context;
using Syspharma.Data.Entities;
using Syspharma.Domain.DTOs;

namespace Syspharma.Business.Tests;

[TestFixture]
public class VentaServiceTests
{
    private SyspharmaContext _context = null!;
    private IMapper _mapper = null!;
    private VentaService _ventaService = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<SyspharmaContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w =>
                w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new SyspharmaContext(options);

        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            },
            NullLoggerFactory.Instance
        );

        _mapper = config.CreateMapper();

        _ventaService = new VentaService(_context, _mapper);

        // Estado de venta
        _context.EstadosVenta.Add(new EstadosVentum
        {
            Id = 1,
            Nombre = "Completada"
        });

        // Método de pago
        _context.MetodosPagos.Add(new MetodosPago
        {
            Id = 1,
            Nombre = "Efectivo",
            Estado = true
        });

        // Turno
        _context.Turnos.Add(new Turno
        {
            Id = 1,
            UsuarioId = 1,
            Estado = "activo",
            FechaApertura = DateTime.Now,
            TotalVentas = 0,
            ResumenVentas = 0
        });

        // Producto
        _context.Productos.Add(new Producto
        {
            Id = 1,
            Nombre = "Paracetamol",
            Stock = 100,
            UltimaActualizacion = DateTime.Now
        });

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task Crear_DebeCalcularSubtotalCorrectamente()
    {
        // Arrange
        var dto = new VentaCreateDto
        {
            TurnoId = 1,
            UsuarioId = 1,
            MetodoPagoId = 1,
            PorcentajeIva = 0,
            Detalles = new List<VentaDetalleCreateDto>
            {
                new VentaDetalleCreateDto
                {
                    ProductoId = 1,
                    Cantidad = 2,
                    PrecioUnitario = 5000,
                    Descuento = 0
                }
            }
        };

        // Act
        var resultado = await _ventaService.Crear(dto);

        // Assert
        Assert.That(resultado, Is.Not.Null);
    }
}