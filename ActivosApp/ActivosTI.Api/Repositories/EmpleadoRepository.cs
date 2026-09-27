using System.Data;
using ActivosTI.Api.Data;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Models;
using Microsoft.Data.SqlClient;

namespace ActivosTI.Api.Repositories;

public class EmpleadoRepository
{
    private readonly ConexionBD _conexionBD;

    public EmpleadoRepository(ConexionBD conexionBD)
    {
        _conexionBD = conexionBD;
    }

    public async Task<int> CrearAsync(
        CrearEmpleadoRequest request)
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Empleado_Crear", conexion);

        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@NumeroEmpleado", SqlDbType.NVarChar, 50).Value = request.NumeroEmpleado;
        comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 150).Value = request.Nombre;
        comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 150).Value = request.Correo;

        await conexion.OpenAsync();

        var resultado = await comando.ExecuteScalarAsync();

        return Convert.ToInt32(resultado);
    }

    public async Task<List<Empleado>> ObtenerTodosAsync()
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Empleado_ObtenerTodos", conexion);

        comando.CommandType = CommandType.StoredProcedure;

        await conexion.OpenAsync();

        await using var reader = await comando.ExecuteReaderAsync();

        var empleados = new List<Empleado>();

        while (await reader.ReadAsync())
        {
            empleados.Add(new Empleado
            {
                EmpleadoId = reader.GetInt32(reader.GetOrdinal("EmpleadoId")),
                NumeroEmpleado = reader.GetString(reader.GetOrdinal("NumeroEmpleado")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Correo = reader.GetString(reader.GetOrdinal("Correo")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
            });
        }

        return empleados;
    }

    public async Task<Empleado?> ObtenerPorIdAsync(
        int empleadoId)
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Empleado_ObtenerPorId", conexion);

        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@EmpleadoId", SqlDbType.Int).Value = empleadoId;

        await conexion.OpenAsync();
        await using var reader = await comando.ExecuteReaderAsync(CommandBehavior.SingleRow);

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return new Empleado
        {
            EmpleadoId = reader.GetInt32(reader.GetOrdinal("EmpleadoId")),
            NumeroEmpleado = reader.GetString(reader.GetOrdinal("NumeroEmpleado")),
            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
            Correo = reader.GetString(reader.GetOrdinal("Correo")),
            Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
        };
    }
}