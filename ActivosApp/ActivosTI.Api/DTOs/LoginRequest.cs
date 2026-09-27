using System.ComponentModel.DataAnnotations;

namespace ActivosTI.Api.DTOs;

public class LoginRequest
{
    [Required]
    public string Usuario { get; set; } = string.Empty;

    [Required]
    public string Contrasena { get; set; } = string.Empty;
}