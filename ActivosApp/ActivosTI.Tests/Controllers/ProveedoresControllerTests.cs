using ActivosTI.Api.Controllers;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ActivosTI.Api.Tests.Controllers
{
    public class ProveedoresControllerTests
    {
        private readonly Mock<IProveedorService> _serviceMock;
        private readonly ProveedoresController _controller;

        public ProveedoresControllerTests()
        {
            _serviceMock = new Mock<IProveedorService>();

            _controller = new ProveedoresController(
                _serviceMock.Object
            );
        }

        [Fact]
        public async Task Crear_DatosValidos_DebeRetornarCreated()
        {
            // Arrange
            var request = new CrearProveedorRequest
            {
                Nombre = "Proveedor Prueba",
                NombreContacto = "Juan Pérez",
                Correo = "proveedor@test.com",
                Telefono = "5555555555",
                ProporcionaCompra = true,
                ProporcionaRenta = false,
                ProporcionaMantenimiento = false
            };

            var proveedorId = 10;

            _serviceMock
                .Setup(x => x.CrearAsync(request))
                .ReturnsAsync(proveedorId);

            // Act
            var resultado = await _controller.Crear(request);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(resultado);

            Assert.Equal(nameof(_controller.ConsultarPorId), created.ActionName);
            Assert.Equal(proveedorId, created.RouteValues!["id"]);

            _serviceMock.Verify(
                x => x.CrearAsync(request),
                Times.Once
            );
        }
    }
}