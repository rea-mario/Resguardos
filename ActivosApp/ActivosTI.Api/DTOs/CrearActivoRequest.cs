using System.ComponentModel.DataAnnotations;

namespace ActivosTI.Api.DTOs;

public class CrearActivoRequest
{
    [Required, MaxLength(50)]
    public string CodigoActivo { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? NumeroSerie { get; set; }

    [Required, MaxLength(100)]
    public string Categoria { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Marca { get; set; }

    [MaxLength(100)]
    public string? Modelo { get; set; }

    [Required]
    public string TipoPropiedad { get; set; } = "Propio";

    public int? ProveedorId { get; set; }

    [MaxLength(30)]
    public string? Estado { get; set; }

    [MaxLength(200)]
    public string? UbicacionActual { get; set; }

    public DateTime? FechaCompra { get; set; }
    public DateTime? FechaFinRenta { get; set; }
}