using ActivosTI.Api.Controllers;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ActivosTI.Api.Tests.Controllers
{
    public class EmpleadosControllerTests
    {
        private readonly Mock<IEmpleadoService> _serviceMock;
        private readonly EmpleadosController _controller;

        public EmpleadosControllerTests()
        {
            _serviceMock = new Mock<IEmpleadoService>();

            _controller = new EmpleadosController(
                _serviceMock.Object
            );
        }

        [Fact]
        public async Task Crear_DatosValidos_DebeRetornarCreated()
        {
            // Arrange
            var request = new CrearEmpleadoRequest
            {
                NumeroEmpleado="1032140",
                Nombre = "Proveedor Prueba",
                Correo="mantonio. rea@gmail.com"
                
            };

            var empleadoId = 1;

            _serviceMock
                .Setup(x => x.CrearAsync(request))
                .ReturnsAsync(empleadoId);

            // Act
            var resultado = await _controller.Crear(request);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(resultado);

            Assert.Equal(nameof(_controller.ObtenerPorId), created.ActionName);
            Assert.Equal(empleadoId, created.RouteValues!["id"]);

            _serviceMock.Verify(
                x => x.CrearAsync(request),
                Times.Once
            );
        }

         [Fact]
        public async Task ObtenerTodos_DebeRetornarOk()
        {
            // Arrange
            var empleados = new List<Models.Empleado>
            {
                new Models.Empleado
                {
                    EmpleadoId = 1,
                    Nombre = "Juan Perez",
                    Activo=true,
                    Correo="juan.perez@gmail.com",
                    NumeroEmpleado="103210",
                    FechaCreacion=DateTime.Now
                },
                new Models.Empleado
                {
                     EmpleadoId = 1,
                    Nombre = "Mario Garcia",
                    Activo=true,
                    Correo="mario.garcia@gmail.com",
                    NumeroEmpleado="103210",
                    FechaCreacion=DateTime.Now
                }
            };

            _serviceMock
                .Setup(x => x.ObtenerTodosAsync())
                .ReturnsAsync(empleados);

            // Act
            var resultado = await _controller.ObtenerTodos();

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(resultado);

            var resultadoEmpleados =
                Assert.IsType<List<Models.Empleado>>(
                    okResult.Value);

            Assert.Equal(2, resultadoEmpleados.Count);

            _serviceMock.Verify(
                x => x.ObtenerTodosAsync(),
                Times.Once);
        }

        [Fact]
        public async Task ObtenerPorId_EmpleadoExiste_DebeRetornarOk()
        {
            // Arrange
            var empleado = new Models.Empleado
            {
                EmpleadoId = 1,
                Nombre = "Juan Perez",
                Activo=true,
                Correo="jua.perez@gmail.com"
            };

            _serviceMock
                .Setup(x => x.ObtenerPorIdAsync(1))
                .ReturnsAsync(empleado);

            // Act
            var resultado =
                await _controller.ObtenerPorId(1);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(resultado);

            var resultadoEmpleado =
                Assert.IsType<Models.Empleado>(
                    okResult.Value);

            Assert.Equal(1, resultadoEmpleado.EmpleadoId);
            Assert.Equal("Juan Perez", resultadoEmpleado.Nombre);

            _serviceMock.Verify(
                x => x.ObtenerPorIdAsync(1),
                Times.Once);
        }

        [Fact]
        public async Task ObtenerPorId_EmpleadoNoExiste_DebeRetornarNotFound()
        {
            // Arrange
            _serviceMock
                .Setup(x => x.ObtenerPorIdAsync(999))
                .ReturnsAsync((Models.Empleado?)null);

            // Act
            var resultado =
                await _controller.ObtenerPorId(999);

            // Assert
            var notFoundResult =
                Assert.IsType<NotFoundObjectResult>(resultado);

            Assert.NotNull(notFoundResult.Value);

            _serviceMock.Verify(
                x => x.ObtenerPorIdAsync(999),
                Times.Once);
        }
    }
}