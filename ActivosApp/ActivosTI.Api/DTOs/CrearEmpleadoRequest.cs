using System.ComponentModel.DataAnnotations;

namespace ActivosTI.Api.DTOs;

public class CrearEmpleadoRequest
{
    [Required]
    [MaxLength(50)]
    public string NumeroEmpleado { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [RegularExpression(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        ErrorMessage = "El correo electrónico no tiene un formato válido."
    )]
    [MaxLength(150)]
    public string Correo { get; set; } = string.Empty;
}