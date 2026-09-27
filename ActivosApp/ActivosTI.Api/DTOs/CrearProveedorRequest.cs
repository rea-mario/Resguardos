using System.ComponentModel.DataAnnotations;

namespace ActivosTI.Api.DTOs;

public class CrearProveedorRequest
{

    [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
    [MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? NombreContacto { get; set; }

    [MaxLength(150)]
    [RegularExpression(
             @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
             ErrorMessage = "El correo electrónico no tiene un formato válido."
         )]
    public string? Correo { get; set; }

    [MaxLength(50)]
    public string? Telefono { get; set; }
    public bool ProporcionaCompra { get; set; }
    public bool ProporcionaRenta { get; set; }
    public bool ProporcionaMantenimiento { get; set; }
    
    [Required(ErrorMessage = "El RFC es obligatorio.")]
    [MaxLength(20)]
    public string RFC { get; set; } = string.Empty;
}
