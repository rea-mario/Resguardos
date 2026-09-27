using System.ComponentModel.DataAnnotations;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Interfaces.Services;
using ActivosTI.Api.Models;
using ActivosTI.Api.Repositories;

namespace ActivosTI.Api.Services;

public class EmpleadoService:IEmpleadoService
{
    private readonly EmpleadoRepository _repository;

    public EmpleadoService(EmpleadoRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> CrearAsync(CrearEmpleadoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NumeroEmpleado))
        {
            throw new ArgumentException("El número de empleado es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(request.Nombre))
        {
            throw new ArgumentException("El nombre es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(request.Correo))
        {
            throw new ArgumentException("El correo es obligatorio.");
        }

        if (request.Correo.Any(char.IsWhiteSpace))
        {
            throw new ArgumentException("El correo electrónico no debe contener espacios en blanco.");
        }

        if (!new EmailAddressAttribute().IsValid(request.Correo))
        {
            throw new ArgumentException("El formato del correo electrónico no es válido.");
        }
        request.Correo = request.Correo.Trim().ToLowerInvariant();
        request.NumeroEmpleado = request.NumeroEmpleado.Trim();
        request.Nombre = request.Nombre.Trim();
        request.Correo = request.Correo.Trim();

        return await _repository.CrearAsync(request);
    }

    public async Task<List<Empleado>> ObtenerTodosAsync()
    {
        return await _repository.ObtenerTodosAsync();
    }

    public async Task<Empleado?> ObtenerPorIdAsync(int empleadoId)
    {
        if (empleadoId <= 0)
        {
            throw new ArgumentException("El EmpleadoId debe ser mayor a cero.");
        }

        return await _repository.ObtenerPorIdAsync(empleadoId);
    }
}