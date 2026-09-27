using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivosTI.Api.Controllers;

[ApiController]
[Route("api/empleados")]

public class EmpleadosController : ControllerBase
{
    private readonly IEmpleadoService _service;

    public EmpleadosController(IEmpleadoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var empleados = await _service.ObtenerTodosAsync();
        return Ok(empleados);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId( int id)
    {
        var empleado = await _service.ObtenerPorIdAsync(id);

        if (empleado == null)
        {
            return NotFound(new
            {
                mensaje = "El empleado no existe."
            });
        }

        return Ok(empleado);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear( [FromBody] CrearEmpleadoRequest request)
    {
        var empleadoId =await _service.CrearAsync(request);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = empleadoId },
            new
            {
                empleadoId,
                mensaje = "Empleado creado correctamente."
            });
    }
}