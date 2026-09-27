using ActivosTI.Api.DTOs;
using ActivosTI.Api.Models;

namespace ActivosTI.Api.Interfaces.Services
{
    public interface IEmpleadoService
    {
        Task<int> CrearAsync(CrearEmpleadoRequest request);
        Task<List<Empleado>> ObtenerTodosAsync();
        Task<Empleado?> ObtenerPorIdAsync(int empleadoId);
    }
}
