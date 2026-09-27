using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivosTI.Api.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProveedoresController : ControllerBase
    {
       private readonly IProveedorService _service;
        public ProveedoresController(IProveedorService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearProveedorRequest request)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
            var proveedorId = await _service.CrearAsync(request);
            return CreatedAtAction(nameof(ConsultarPorId), new { id = proveedorId }, new { proveedorId, mensaje = "Proveedor creado correctamente." });
        }
        [HttpGet]
        public async Task<IActionResult> Consultar([FromQuery] int? proveedorId = null, [FromQuery] string? nombre = null, [FromQuery] string? rfc = null, [FromQuery] bool? activo = null, [FromQuery] bool? proporcionaCompra = null, [FromQuery] bool? proporcionaRenta = null, [FromQuery] bool? proporcionaMantenimiento = null)
        {
            var proveedores = await _service.ConsultarAsync(proveedorId, nombre, rfc, activo, proporcionaCompra, proporcionaRenta, proporcionaMantenimiento);
            return Ok(proveedores);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> ConsultarPorId(int id)
        {
            var proveedores = await _service.ConsultarAsync(proveedorId: id);
            var proveedor = proveedores.FirstOrDefault();
            if (proveedor == null)
            {
                return NotFound(new { mensaje = "El proveedor no existe." });
            }
            return Ok(proveedor);
        }
    }
