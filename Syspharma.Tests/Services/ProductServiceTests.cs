using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Syspharma.Business.Services;
using Syspharma.Data.Repositories;
using Syspharma.Domain.DTOs;
using Xunit;

namespace Syspharma.Tests.Services
{
    public class ProductServiceTests
    {
        // Prueba: ObtenerTodos delega al repositorio y devuelve la lista esperada
        [Fact]
        public async Task ObtenerTodos_DelegaAlRepositorio_RetornaLista()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            var expected = new List<ProductoDto>
            {
                new ProductoDto { Id = 1, Nombre = "A" },
                new ProductoDto { Id = 2, Nombre = "B" }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(expected);
            var service = new ProductoService(mockRepo.Object);

            // Act
            var result = await service.ObtenerTodos();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            mockRepo.Verify(r => r.ObtenerTodos(), Times.Once);
        }

        // Prueba: ObtenerPorId cuando existe el producto
        [Fact]
        public async Task ObtenerPorId_Existente_RetornaProducto()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            var dto = new ProductoDto { Id = 10, Nombre = "Test" };
            mockRepo.Setup(r => r.ObtenerPorId(10)).ReturnsAsync(dto);
            var service = new ProductoService(mockRepo.Object);

            // Act
            var result = await service.ObtenerPorId(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result!.Id);
            Assert.Equal("Test", result.Nombre);
            mockRepo.Verify(r => r.ObtenerPorId(10), Times.Once);
        }

        // Prueba: ObtenerPorId cuando no existe (null)
        [Fact]
        public async Task ObtenerPorId_Inexistente_RetornaNull()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            mockRepo.Setup(r => r.ObtenerPorId(It.IsAny<int>())).ReturnsAsync((ProductoDto?)null);
            var service = new ProductoService(mockRepo.Object);

            // Act
            var result = await service.ObtenerPorId(999);

            // Assert
            Assert.Null(result);
            mockRepo.Verify(r => r.ObtenerPorId(999), Times.Once);
        }

        // Prueba: Crear delega al repositorio y retorna el producto creado
        [Fact]
        public async Task Crear_InvocaRepositorio_DevuelveProducto()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            var createDto = new ProductoCreateDto { Nombre = "Nuevo", CategoriaId = 1, Precio = 9.5m };
            var returned = new ProductoDto { Id = 5, Nombre = "Nuevo", CategoriaId = 1, Precio = 9.5m };
            mockRepo.Setup(r => r.Crear(It.IsAny<ProductoCreateDto>())).ReturnsAsync(returned);
            var service = new ProductoService(mockRepo.Object);

            // Act
            var result = await service.Crear(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal("Nuevo", result.Nombre);
            mockRepo.Verify(r => r.Crear(It.Is<ProductoCreateDto>(d => d.Nombre == "Nuevo" && d.CategoriaId == 1 && d.Precio == 9.5m)), Times.Once);
        }

        // Prueba: Crear cuando el repositorio lanza una excepción se propaga
        [Fact]
        public async Task Crear_RepositorioLanzaExcepcion_Propaga()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            var createDto = new ProductoCreateDto { Nombre = "X", CategoriaId = 1, Precio = 1m };
            mockRepo.Setup(r => r.Crear(It.IsAny<ProductoCreateDto>())).ThrowsAsync(new Exception("db error"));
            var service = new ProductoService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.Crear(createDto));
            mockRepo.Verify(r => r.Crear(It.IsAny<ProductoCreateDto>()), Times.Once);
        }

        // Prueba: Actualizar delega al repositorio y devuelve el resultado
        [Fact]
        public async Task Actualizar_Exitoso_RetornaProductoActualizado()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            var updateDto = new ProductoUpdateDto { Id = 7, Nombre = "Mod", CategoriaId = 2, Precio = 12m };
            var returned = new ProductoDto { Id = 7, Nombre = "Mod", CategoriaId = 2, Precio = 12m };
            mockRepo.Setup(r => r.Actualizar(It.IsAny<ProductoUpdateDto>())).ReturnsAsync(returned);
            var service = new ProductoService(mockRepo.Object);

            // Act
            var result = await service.Actualizar(updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("Mod", result.Nombre);
            mockRepo.Verify(r => r.Actualizar(It.Is<ProductoUpdateDto>(d => d.Id == 7 && d.Nombre == "Mod")), Times.Once);
        }

        // Prueba: Actualizar cuando el repositorio lanza excepción (producto no encontrado, etc.)
        [Fact]
        public async Task Actualizar_RepositorioLanzaExcepcion_Propaga()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            var updateDto = new ProductoUpdateDto { Id = 999, Nombre = "Nada", CategoriaId = 1, Precio = 1m };
            mockRepo.Setup(r => r.Actualizar(It.IsAny<ProductoUpdateDto>())).ThrowsAsync(new Exception("not found"));
            var service = new ProductoService(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.Actualizar(updateDto));
            mockRepo.Verify(r => r.Actualizar(It.IsAny<ProductoUpdateDto>()), Times.Once);
        }

        // Prueba: CambiarEstado devuelve true cuando el repositorio indica éxito
        [Fact]
        public async Task CambiarEstado_RepositorioRetornaTrue_RetornaTrue()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            mockRepo.Setup(r => r.CambiarEstado(1, true)).ReturnsAsync(true);
            var service = new ProductoService(mockRepo.Object);

            // Act
            var result = await service.CambiarEstado(1, true);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.CambiarEstado(1, true), Times.Once);
        }

        // Prueba: CambiarEstado devuelve false cuando el repositorio indica fallo (producto inexistente)
        [Fact]
        public async Task CambiarEstado_RepositorioRetornaFalse_RetornaFalse()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            mockRepo.Setup(r => r.CambiarEstado(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(false);
            var service = new ProductoService(mockRepo.Object);

            // Act
            var result = await service.CambiarEstado(999, false);

            // Assert
            Assert.False(result);
            mockRepo.Verify(r => r.CambiarEstado(999, false), Times.Once);
        }

        // Prueba: Eliminar devuelve true/false según el repositorio
        [Fact]
        public async Task Eliminar_Exitoso_Y_Fallo_VerificarLlamadas()
        {
            // Arrange
            var mockRepo = new Mock<IProductoRepository>();
            mockRepo.Setup(r => r.Eliminar(2)).ReturnsAsync(true);
            mockRepo.Setup(r => r.Eliminar(999)).ReturnsAsync(false);
            var service = new ProductoService(mockRepo.Object);

            // Act
            var ok = await service.Eliminar(2);
            var no = await service.Eliminar(999);

            // Assert
            Assert.True(ok);
            Assert.False(no);
            mockRepo.Verify(r => r.Eliminar(2), Times.Once);
            mockRepo.Verify(r => r.Eliminar(999), Times.Once);
        }
    }
}
