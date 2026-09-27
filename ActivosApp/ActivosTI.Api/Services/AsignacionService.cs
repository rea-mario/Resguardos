using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using ActivosTI.Api.Models;
using ActivosTI.Api.Repositories;

namespace ActivosTI.Api.Services;

public class AsignacionService:IAsignacionService
{
    private readonly AsignacionRepository _repository;

    public AsignacionService(AsignacionRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> AsignarAsync(int activoId, AsignarActivoRequest request, int usuarioId)
    {
        ValidarIds(activoId, usuarioId);

        if (request.EmpleadoId <= 0)
        {
            throw new ArgumentException("El EmpleadoId debe ser mayor a cero.");
        }

        request.Observaciones = Normalizar(request.Observaciones);

        return await _repository.AsignarAsync(activoId, request, usuarioId);
    }

    public async Task DevolverAsync(int activoId, DevolverActivoRequest request, int usuarioId)
    {
        ValidarIds(activoId, usuarioId);

        if (string.IsNullOrWhiteSpace(request.CondicionDevolucion))
        {
            throw new ArgumentException("La condición de devolución es obligatoria.");
        }

        request.CondicionDevolucion = request.CondicionDevolucion.Trim();
        request.Observaciones = Normalizar(request.Observaciones);

        await _repository.DevolverAsync(activoId, request, usuarioId);
    }

    public async Task<List<Asignacion>> ObtenerPorActivoAsync(int activoId)
    {
        if (activoId <= 0)
        {
            throw new ArgumentException("El ActivoId debe ser mayor a cero.");
        }

        return await _repository.ObtenerPorActivoAsync(activoId);
    }

    public async Task<List<Asignacion>> ObtenerPorEmpleadoAsync(int empleadoId)
    {
        if (empleadoId <= 0)
        {
            throw new ArgumentException("El EmpleadoId debe ser mayor a cero.");
        }

        return await _repository.ObtenerPorEmpleadoAsync(empleadoId);
    }

    private static void ValidarIds(int activoId, int usuarioId)
    {
        if (activoId <= 0)
        {
            throw new ArgumentException("El ActivoId debe ser mayor a cero.");
        }

        if (usuarioId <= 0)
        {
            throw new ArgumentException("El UsuarioId no es válido.");
        }
    }

    private static string? Normalizar(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}