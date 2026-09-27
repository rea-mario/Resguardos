using ActivosTI.Api.DTOs;
using ActivosTI.Api.Models;

namespace ActivosTI.Api.Interfaces.Services
{
    public interface IProveedorService
    {
        Task<int> CrearAsync(CrearProveedorRequest request);
        Task<List<ProveedorDto>> ConsultarAsync(int? proveedorId = null, string? nombre = null, string? rfc = null, bool? activo = null, bool? proporcionaCompra = null, bool? proporcionaRenta = null, bool? proporcionaMantenimiento = null);
    }
}
