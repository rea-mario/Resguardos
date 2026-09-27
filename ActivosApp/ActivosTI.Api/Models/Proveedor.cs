namespace ActivosTI.Api.Models;

public class Proveedor
{
   public int ProveedorId { get; set; } 
   public string Nombre { get; set; } = string.Empty; 
   public string? NombreContacto { get; set; } 
   public string? Correo { get; set; } 
   public string? Telefono { get; set; } 
   public bool ProporcionaCompra { get; set; } 
   public bool ProporcionaRenta { get; set; } 
   public bool ProporcionaMantenimiento { get; set; } 
   public string RFC { get; set; } = string.Empty; 
   public bool Activo { get; set; } 
   public DateTime FechaCreacion { get; set; }
}