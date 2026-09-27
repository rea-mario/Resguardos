using ActivosTI.Api.DTOs;
using ActivosTI.Api.Models;

namespace ActivosTI.Api.Interfaces.Services
{
    public interface IActivoService
    {
        Task<int> CrearAsync(CrearActivoRequest request);
        Task<Activo?> ObtenerPorIdAsync(int activoId);
         Task<IEnumerable<Activo>> ConsultarAsync(int? id = null, string? codigoActivo = null, string? categoria = null, string? estado = null, string? tipoPropiedad = null, int? idProveedor = null);
         Task<List<Activo>> ObtenerPaginadoAsync(string? search, string? estado, string? categoria, int pagina, int registrosPorPagina);
         Task ActualizarAsync(int activoId, CrearActivoRequest request, int usuarioId);
        
    }
}
