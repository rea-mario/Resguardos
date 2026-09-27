using ActivosTI.Api.Controllers;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ActivosTI.Api.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _serviceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _serviceMock = new Mock<IAuthService>();

            _controller = new AuthController(
                _serviceMock.Object
            );
        }

        [Fact]
        public async Task Login_CredencialesValidas_DebeRetornarOk()
        {
            // Arrange
            var request = new LoginRequest
            {
                Usuario = "admin",
                Contrasena = "123456"
            };

            var respuesta = new LoginResponse();

            _serviceMock
                .Setup(x => x.LoginAsync(request))
                .ReturnsAsync(respuesta);

            // Act
            var resultado = await _controller.Login(request);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, ok.StatusCode);
            Assert.Same(respuesta, ok.Value);

            _serviceMock.Verify(
                x => x.LoginAsync(request),
                Times.Once);
        }
    }
}