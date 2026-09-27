using System.Security.Claims;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivosTI.Api.Controllers;

[ApiController]
[Route("api/asignaciones")]
[Authorize]
public class AsignacionesController : ControllerBase
{
    private readonly IAsignacionService _service;

    public AsignacionesController(IAsignacionService service)
    {
        _service = service;
    }

    [HttpPost("activos/{activoId:int}")]
    [Authorize(Roles = "Administrador,Operador")]
    public async Task<IActionResult> Asignar(int activoId, [FromBody] AsignarActivoRequest request)
    {
        int usuarioId = ObtenerUsuarioId();
        long asignacionId = await _service.AsignarAsync(activoId, request, usuarioId);

        return Ok(new
        {
            asignacionId,
            activoId,
            request.EmpleadoId,
            mensaje = "Activo asignado correctamente."
        });
    }

    [HttpPost("activos/{activoId:int}/devolver")]
    [Authorize(Roles = "Administrador,Operador")]
    public async Task<IActionResult> Devolver(int activoId, [FromBody] DevolverActivoRequest request)
    {
        int usuarioId = ObtenerUsuarioId();

        await _service.DevolverAsync(
            activoId,
            request,
            usuarioId);

        return Ok(new
        {
            activoId,
            mensaje = "Activo devuelto correctamente."
        });
    }

    [HttpGet("activos/{activoId:int}")]
    public async Task<IActionResult> ObtenerPorActivo(int activoId)
    {
        var asignaciones = await _service.ObtenerPorActivoAsync(activoId);
        return Ok(asignaciones);
    }

    [HttpGet("empleados/{empleadoId:int}")]
    public async Task<IActionResult> ObtenerPorEmpleado(
        int empleadoId)
    {
        var asignaciones = await _service.ObtenerPorEmpleadoAsync(empleadoId);
        return Ok(asignaciones);
    }

    private int ObtenerUsuarioId()
    {
        var valor = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(valor, out int usuarioId))
        {
            throw new UnauthorizedAccessException("No se pudo identificar al usuario.");
        }

        return usuarioId;
    }
}