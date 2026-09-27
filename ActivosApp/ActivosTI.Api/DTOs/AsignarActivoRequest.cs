using System.ComponentModel.DataAnnotations;

namespace ActivosTI.Api.DTOs;

public class AsignarActivoRequest
{
    [Range(1, int.MaxValue)]
    public int EmpleadoId { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }
}