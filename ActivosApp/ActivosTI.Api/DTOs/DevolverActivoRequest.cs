using System.ComponentModel.DataAnnotations;

namespace ActivosTI.Api.DTOs;

public class DevolverActivoRequest
{
    [Required]
    [MaxLength(200)]
    public string CondicionDevolucion { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Observaciones { get; set; }
}