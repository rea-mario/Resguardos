using ActivosTI.Api.DTOs;
using ActivosTI.Api.Models;

namespace ActivosTI.Api.Interfaces.Services
{
    public interface IAsignacionService
    {
        Task<long> AsignarAsync(int activoId, AsignarActivoRequest request, int usuarioId);
        Task DevolverAsync(int activoId, DevolverActivoRequest request, int usuarioId);
        Task<List<Asignacion>> ObtenerPorActivoAsync(int activoId);
        Task<List<Asignacion>> ObtenerPorEmpleadoAsync(int empleadoId);
        
    }
}
