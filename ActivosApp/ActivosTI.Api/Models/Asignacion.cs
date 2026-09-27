namespace ActivosTI.Api.Models;

public class Asignacion
{
    public long AsignacionId { get; set; }
    public int ActivoId { get; set; }
    public string CodigoActivo { get; set; } = string.Empty;
    public int EmpleadoId { get; set; }
    public string NumeroEmpleado { get; set; } = string.Empty;
    public string NombreEmpleado { get; set; } = string.Empty;
    public DateTime FechaAsignacion { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public string? CondicionDevolucion { get; set; }
    public int AsignadoPor { get; set; }
    public int? DevueltoPor { get; set; }
    public string? Observaciones { get; set; }
    public bool Activa => !FechaDevolucion.HasValue;
}