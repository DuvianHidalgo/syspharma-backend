using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Syspharma.Business.Services;
using Syspharma.Data.Repositories;
using Syspharma.Domain.DTOs;
using Xunit;

namespace Syspharma.Tests.Services
{
    public class PurchaseServiceTests
    {
        // Prueba: ObtenerTodos delega al repositorio y retorna lista
        [Fact]
        public async Task ObtenerTodos_DelegaAlRepositorio_RetornaLista()
        {
            // Arrange
            var mockRepo = new Mock<ICompraRepository>();
            var expected = new List<CompraDto>
            {
                new CompraDto { Id = 1, NumeroCompra = "C1" },
                new CompraDto { Id = 2, NumeroCompra = "C2" }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(expected);
            var service = new CompraService(mockRepo.Object);

            // Act
            var result = await service.ObtenerTodos();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            mockRepo.Verify(r => r.ObtenerTodos(), Times.Once);
        }

        // Prueba: ObtenerPorId cuando existe
        [Fact]
        public async Task ObtenerPorId_Existente_RetornaCompra()
        {
            // Arrange
            var mockRepo = new Mock<ICompraRepository>();
            var dto = new CompraDto { Id = 42, NumeroCompra = "COM-42" };
            mockRepo.Setup(r => r.ObtenerPorId(42)).ReturnsAsync(dto);
            var service = new CompraService(mockRepo.Object);

            // Act
            var result = await service.ObtenerPorId(42);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(42, result!.Id);
            mockRepo.Verify(r => r.ObtenerPorId(42), Times.Once);
        }

        // Prueba: ObtenerPorId inexistente retorna null
        [Fact]
        public async Task ObtenerPorId_Inexistente_RetornaNull()
        {
            // Arrange
            var mockRepo = new Mock<ICompraRepository>();
            mockRepo.Setup(r => r.ObtenerPorId(It.IsAny<int>())).ReturnsAsync((CompraDto?)null);
            var service = new CompraService(mockRepo.Object);

            // Act
            var result = await service.ObtenerPorId(9999);

            // Assert
            Assert.Null(result);
            mockRepo.Verify(r => r.ObtenerPorId(9999), Times.Once);
        }

        // Prueba: Crear delega al repositorio, verifica detalles y totales delegados
        [Fact]
        public async Task Crear_InvocaRepositorio_DevuelveCompra()
        {
            // Arrange
            var mockRepo = new Mock<ICompraRepository>();
            var createDto = new CompraCreateDto
            {
                ProveedorId = 1,
                UsuarioId = 2,
                Detalles = new List<CompraDetalleCreateDto>
                {
                    new CompraDetalleCreateDto { ProductoId = 1, Cantidad = 2, PrecioUnitario = 5m }
                }
            };
            var returned = new CompraDto { Id = 100, NumeroCompra = "COM-100", ProveedorId = 1, UsuarioId = 2, Detalles = new List<CompraDetalleDto>() };
            mockRepo.Setup(r => r.Crear(It.IsAny<CompraCreateDto>())).ReturnsAsync(returned);
            var service = new CompraService(mockRepo.Object);

            // Act
            var result = await service.Crear(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.Id);
            mockRepo.Verify(r => r.Crear(It.Is<CompraCreateDto>(d => d.ProveedorId == 1 && d.UsuarioId == 2 && d.Detalles.Count == 1)), Times.Once);
        }

        // Prueba: Crear cuando el repositorio lanza excepción (por ejemplo estado 'Pendiente' no encontrado)
        [Fact]
        public async Task Crear_RepositorioLanzaExcepcion_Propaga()
        {
            // Arrange
            var mockRepo = new Mock<ICompraRepository>();
            var createDto = new CompraCreateDto { ProveedorId = 1, UsuarioId = 1 };
            mockRepo.Setup(r => r.Crear(It.IsAny<CompraCreateDto>())).ThrowsAsync(new Exception("Estado 'Pendiente' no encontrado"));
            var service = new CompraService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.Crear(createDto));
            mockRepo.Verify(r => r.Crear(It.IsAny<CompraCreateDto>()), Times.Once);
        }

        // Prueba: Actualizar exitoso delega al repositorio y devuelve resultado
        [Fact]
        public async Task Actualizar_Exitoso_RetornaCompraActualizada()
        {
            // Arrange
            var mockRepo = new Mock<ICompraRepository>();
            var updateDto = new CompraUpdateDto
            {
                Id = 11,
                ProveedorId = 2,
                EstadoId = 1,
                Detalles = new List<CompraDetalleCreateDto>
                {
                    new CompraDetalleCreateDto { ProductoId = 1, Cantidad = 1, PrecioUnitario = 2m }
                }
            };
            var returned = new CompraDto { Id = 11, NumeroCompra = "COM-11", ProveedorId = 2, EstadoId = 1 };
            mockRepo.Setup(r => r.Actualizar(It.IsAny<CompraUpdateDto>())).ReturnsAsync(returned);
            var service = new CompraService(mockRepo.Object);

            // Act
            var result = await service.Actualizar(updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(11, result.Id);
            mockRepo.Verify(r => r.Actualizar(It.Is<CompraUpdateDto>(d => d.Id == 11 && d.ProveedorId == 2)), Times.Once);
        }

        // Prueba: Actualizar cuando repositorio lanza excepcion (compra no encontrada)
        [Fact]
        public async Task Actualizar_RepositorioLanzaExcepcion_Propaga()
        {
            // Arrange
            var mockRepo = new Mock<ICompraRepository>();
            var updateDto = new CompraUpdateDto { Id = 999, ProveedorId = 1, EstadoId = 1 };
            mockRepo.Setup(r => r.Actualizar(It.IsAny<CompraUpdateDto>())).ThrowsAsync(new Exception("Compra no encontrada"));
            var service = new CompraService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.Actualizar(updateDto));
            mockRepo.Verify(r => r.Actualizar(It.IsAny<CompraUpdateDto>()), Times.Once);
        }

        // Prueba: CambiarEstado devuelve true/false según repositorio
        [Fact]
        public async Task CambiarEstado_VerificarResultados()
        {
            // Arrange
            var mockRepo = new Mock<ICompraRepository>();
            mockRepo.Setup(r => r.CambiarEstado(1, 2)).ReturnsAsync(true);
            mockRepo.Setup(r => r.CambiarEstado(999, 2)).ReturnsAsync(false);
            var service = new CompraService(mockRepo.Object);

            // Act
            var ok = await service.CambiarEstado(1, 2);
            var no = await service.CambiarEstado(999, 2);

            // Assert
            Assert.True(ok);
            Assert.False(no);
            mockRepo.Verify(r => r.CambiarEstado(1, 2), Times.Once);
            mockRepo.Verify(r => r.CambiarEstado(999, 2), Times.Once);
        }

        // Prueba: Eliminar delega y retorna según repositorio
        [Fact]
        public async Task Eliminar_VerificarResultados()
        {
            // Arrange
            var mockRepo = new Mock<ICompraRepository>();
            mockRepo.Setup(r => r.Eliminar(5)).ReturnsAsync(true);
            mockRepo.Setup(r => r.Eliminar(777)).ReturnsAsync(false);
            var service = new CompraService(mockRepo.Object);

            // Act
            var ok = await service.Eliminar(5);
            var no = await service.Eliminar(777);

            // Assert
            Assert.True(ok);
            Assert.False(no);
            mockRepo.Verify(r => r.Eliminar(5), Times.Once);
            mockRepo.Verify(r => r.Eliminar(777), Times.Once);
        }

        // Prueba: ObtenerEstados delega al repositorio y retorna lista de estados
        [Fact]
        public async Task ObtenerEstados_RetornaLista()
        {
            // Arrange
            var mockRepo = new Mock<ICompraRepository>();
            var estados = new List<object> { new { Id = 1, Nombre = "Pendiente" }, new { Id = 2, Nombre = "Completado" } };
            mockRepo.Setup(r => r.ObtenerEstados()).ReturnsAsync(estados.Cast<object>().ToList());
            var service = new CompraService(mockRepo.Object);

            // Act
            var result = await service.ObtenerEstados();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            mockRepo.Verify(r => r.ObtenerEstados(), Times.Once);
        }
    }
}
