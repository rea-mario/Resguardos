namespace ActivosTI.Api.Models;

public class Usuario
{
    public int UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string HashContrasena { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public int IntentosFallidos { get; set; }
    public DateTime? BloqueadoHasta { get; set; }
    public bool Activo { get; set; }
}