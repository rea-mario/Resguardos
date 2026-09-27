

using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using ActivosTI.Api.Repositories;

namespace ActivosTI.Api.Services;

public class ProveedorService : IProveedorService
{
   private readonly ProveedorRepository _repository;
    public ProveedorService(ProveedorRepository repository)
    {
        _repository = repository;
    }
    public async Task<int> CrearAsync(CrearProveedorRequest request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(request.Nombre))
        {
            throw new ArgumentException("El nombre del proveedor es obligatorio.");
        }
        if (string.IsNullOrWhiteSpace(request.RFC))
        {
            throw new ArgumentException("El RFC del proveedor es obligatorio.");
        }
        return await _repository.CrearAsync(request);
    }

    public async Task<List<ProveedorDto>> ConsultarAsync(int? proveedorId = null, string? nombre = null, string? rfc = null, bool? activo = null, bool? proporcionaCompra = null, bool? proporcionaRenta = null, bool? proporcionaMantenimiento = null)
    {
        return await _repository.ConsultarAsync(proveedorId, nombre, rfc, activo, proporcionaCompra, proporcionaRenta, proporcionaMantenimiento);
    }
}
