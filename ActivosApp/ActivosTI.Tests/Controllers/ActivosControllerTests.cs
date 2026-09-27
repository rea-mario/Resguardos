using ActivosTI.Api.Controllers;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ActivosTI.Api.Tests.Controllers;

public class ActivosControllerTests
{
    private readonly Mock<IActivoService> _serviceMock;
    private readonly ActivosController _controller;

    public ActivosControllerTests()
    {
        _serviceMock = new Mock<IActivoService>();

        _controller = new ActivosController(
            _serviceMock.Object
        );
    }

    // Aquí agregaremos las pruebas.
    [Fact]
    public async Task Crear_ActivoValido_DebeRetornarCreated()
    {
        // Arrange
        var request = new CrearActivoRequest
        {
            // Agrega aquí las propiedades obligatorias
            // de tu DTO CrearActivoRequest.
        };

        int activoId = 10;

        _serviceMock
            .Setup(x => x.CrearAsync(request))
            .ReturnsAsync(activoId);

        // Act
        var resultado = await _controller.Crear(request);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(
            resultado);

        Assert.Equal(
            nameof(_controller.ConsultarPorId),
            created.ActionName);

        Assert.Equal(activoId, created.RouteValues!["id"]);

        _serviceMock.Verify(
            x => x.CrearAsync(request),
            Times.Once);
    }
    [Fact]
    public async Task Crear_ModelStateInvalido_DebeRetornarBadRequest()
    {
        // Arrange
        var request = new CrearActivoRequest();

        _controller.ModelState.AddModelError(
            "CodigoActivo",
            "El código del activo es obligatorio.");

        // Act
        var resultado = await _controller.Crear(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);

        _serviceMock.Verify(
            x => x.CrearAsync(It.IsAny<CrearActivoRequest>()),
            Times.Never);
    }
    [Fact]
    public async Task Consultar_DebeRetornarOkConActivos()
    {
        // Arrange
        var activos = new List<Models.Activo>
    {
        new Models.Activo
        {
            ActivoId= 4,
            CodigoActivo= "ACT-0004",
            NumeroSerie= "DELL-MON-001",
            Categoria= "Monitor",
            Marca= "Dell",
            Modelo= "P2422H",
            TipoPropiedad= "Propio",
            ProveedorId= 1,
            Estado= "Disponible",
            UbicacionActual= "Almacen TI",
            FechaCompra= DateTime.Parse( "2025-01-20T00:00:00"),
            FechaFinRenta= null
        },
        new Models.Activo
        {
            ActivoId= 4,
            CodigoActivo= "ACT-0005",
            NumeroSerie= "HP-PRN-001",
            Categoria= "Impresora",
            Marca= "HP",
            Modelo= "LaserJet Pro M404",
            TipoPropiedad= "Propio",
            ProveedorId= 1,
            Estado= "Mantenimiento",
            UbicacionActual= "Almacen TI",
            FechaCompra= DateTime.Parse( "2025-01-20T00:00:00"),
        }
    };

        _serviceMock
            .Setup(x => x.ConsultarAsync(
                null, null, null, null, null, null))
            .ReturnsAsync(activos);

        // Act
        var resultado = await _controller.Consultar();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(resultado);

        Assert.Same(activos, ok.Value);

        _serviceMock.Verify(
            x => x.ConsultarAsync(
                null, null, null, null, null, null),
            Times.Once);
    }

}