using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivosTI.Api.Controllers;

[ApiController]
[Route("api/Activos")]
[Authorize]

public class ActivosController : ControllerBase
{
    private readonly IActivoService _activoService;

    public ActivosController(IActivoService activoService)
    {
        _activoService = activoService;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearActivoRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = await _activoService.CrearAsync(request);

        return CreatedAtAction(
            nameof(ConsultarPorId),
            new { id },
            new
            {
                id,
                mensaje = "Activo creado correctamente."
            });
    }

    [HttpGet]
    public async Task<IActionResult> Consultar([FromQuery] int? id = null, [FromQuery] string? codigoActivo = null, [FromQuery] string? categoria = null, [FromQuery] string? estado = null, [FromQuery] string? tipoPropiedad = null, [FromQuery] int? idProveedor = null)
    {
        var activos = await _activoService.ConsultarAsync(id, codigoActivo, categoria, estado, tipoPropiedad, idProveedor);
        return Ok(activos);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> ConsultarPorId(int id)
    {
        var activos = await _activoService.ConsultarAsync(id, null, null, null, null, null);
        var activo = activos.FirstOrDefault();

        if (activo == null)
        {
            return NotFound(new
            {
                mensaje = "El activo no existe."
            });
        }

        return Ok(activo);
    }


}