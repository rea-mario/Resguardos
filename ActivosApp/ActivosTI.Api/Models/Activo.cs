namespace ActivosTI.Api.Models;

public class Activo
{
    public int ActivoId { get; set; }
    public string CodigoActivo { get; set; } = string.Empty;
    public string? NumeroSerie { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string TipoPropiedad { get; set; } = string.Empty;
    public int? ProveedorId { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? UbicacionActual { get; set; }
    public DateTime? FechaCompra { get; set; }
    public DateTime? FechaFinRenta { get; set; }
}