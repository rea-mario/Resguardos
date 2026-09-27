using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using ActivosTI.Api.Models;
using ActivosTI.Api.Repositories;

namespace ActivosTI.Api.Services;

public class ActivoService:IActivoService
{
    private readonly ActivoRepository _activoRepository;

    public ActivoService(ActivoRepository activoRepository)
    {
        _activoRepository = activoRepository;
    }

    public async Task<int> CrearAsync(CrearActivoRequest request)
    {
        ValidarDatos(request);
        return await _activoRepository.CrearAsync(request);
    }

    public async Task<Activo?> ObtenerPorIdAsync(int activoId)
    {
        if (activoId <= 0)
        {
            throw new ArgumentException("El ActivoId debe ser mayor a cero.");
        }

        return await _activoRepository.ObtenerPorIdAsync(activoId);
    }

    public async Task<IEnumerable<Activo>> ConsultarAsync(int? id = null, string? codigoActivo = null, string? categoria = null, string? estado = null, string? tipoPropiedad = null, int? idProveedor = null)
    {
        return await _activoRepository.ConsultarAsync(id, codigoActivo, categoria, estado, tipoPropiedad, idProveedor);
    }

    public async Task<List<Activo>> ObtenerPaginadoAsync(string? search, string? estado, string? categoria, int pagina, int registrosPorPagina)
    {
        if (pagina < 1)
        {
            pagina = 1;
        }

        if (registrosPorPagina < 1)
        {
            registrosPorPagina = 10;
        }

        if (registrosPorPagina > 100)
        {
            registrosPorPagina = 100;
        }

        search = Normalizar(search);
        estado = Normalizar(estado);
        categoria = Normalizar(categoria);

        return await _activoRepository.ObtenerPaginadoAsync(search, estado, categoria, pagina, registrosPorPagina);
    }

    public async Task ActualizarAsync(int activoId, CrearActivoRequest request, int usuarioId)
    {
        if (activoId <= 0)
        {
            throw new ArgumentException(
                "El ActivoId debe ser mayor a cero.");
        }

        ValidarDatos(request);

        var activo = await _activoRepository.ObtenerPorIdAsync(activoId);

        if (activo == null)
        {
            throw new KeyNotFoundException("El activo no existe.");
        }

        if (activo.Estado == "Retirado")
        {
            throw new InvalidOperationException("Un activo retirado no puede modificarse.");
        }

        await _activoRepository.ActualizarAsync(activoId, request, usuarioId);
    }

    private static void ValidarDatos(CrearActivoRequest request)
    {
        if (string.IsNullOrWhiteSpace(
            request.CodigoActivo))
        {
            throw new ArgumentException("El código del activo es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(request.Categoria))
        {
            throw new ArgumentException("La categoría es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(request.TipoPropiedad))
        {
            throw new ArgumentException("El tipo de propiedad es obligatorio.");
        }

        if (request.TipoPropiedad != "Propio" && request.TipoPropiedad != "Arrendado")
        {
            throw new ArgumentException("El tipo de propiedad debe ser " + "'Propio' o 'Arrendado'.");
        }

        if (request.TipoPropiedad == "Arrendado" && !request.ProveedorId.HasValue)
        {
            throw new ArgumentException("Un activo arrendado debe tener proveedor.");
        }

        if (request.FechaFinRenta.HasValue && request.TipoPropiedad != "Arrendado")
        {
            throw new ArgumentException("La fecha de fin de renta solo aplica " + "a activos arrendados.");
        }

        if (request.FechaCompra.HasValue && request.FechaFinRenta.HasValue && request.FechaFinRenta.Value.Date < request.FechaCompra.Value.Date)
        {
            throw new ArgumentException("La fecha de fin de renta no puede ser " + "anterior a la fecha de compra.");
        }
    }

    private static string? Normalizar(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return null;
        }

        return valor.Trim();
    }
}