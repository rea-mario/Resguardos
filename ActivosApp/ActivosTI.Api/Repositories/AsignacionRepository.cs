using System.Data;
using ActivosTI.Api.Data;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Models;
using Microsoft.Data.SqlClient;

namespace ActivosTI.Api.Repositories;

public class AsignacionRepository
{
    private readonly ConexionBD _conexionBD;

    public AsignacionRepository(ConexionBD conexionBD)
    {
        _conexionBD = conexionBD;
    }

    public async Task<long> AsignarAsync(int activoId, AsignarActivoRequest request, int usuarioId)
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Asignacion_Crear", conexion);

        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@ActivoId", SqlDbType.Int).Value = activoId;
        comando.Parameters.Add("@EmpleadoId", SqlDbType.Int).Value = request.EmpleadoId;
        comando.Parameters.Add("@AsignadoPor", SqlDbType.Int).Value = usuarioId;
        comando.Parameters.Add("@Observaciones", SqlDbType.NVarChar, 500).Value = (object?)request.Observaciones ?? DBNull.Value;

        await conexion.OpenAsync();

        var resultado = await comando.ExecuteScalarAsync();

        return Convert.ToInt64(resultado);
    }

    public async Task DevolverAsync(int activoId, DevolverActivoRequest request, int usuarioId)
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Asignacion_Devolver", conexion);

        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@ActivoId", SqlDbType.Int).Value = activoId;
        comando.Parameters.Add("@DevueltoPor", SqlDbType.Int).Value = usuarioId;
        comando.Parameters.Add("@CondicionDevolucion", SqlDbType.NVarChar, 200).Value = request.CondicionDevolucion;
        comando.Parameters.Add("@Observaciones", SqlDbType.NVarChar, 500).Value = (object?)request.Observaciones ?? DBNull.Value;

        await conexion.OpenAsync();
        await comando.ExecuteNonQueryAsync();
    }

    public async Task<List<Asignacion>> ObtenerPorActivoAsync(
        int activoId)
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Asignacion_ObtenerPorActivo", conexion);

        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@ActivoId", SqlDbType.Int).Value = activoId;

        await conexion.OpenAsync();
        await using var reader = await comando.ExecuteReaderAsync();

        var asignaciones = new List<Asignacion>();

        while (await reader.ReadAsync())
        {
            asignaciones.Add(MapearAsignacion(reader));
        }

        return asignaciones;
    }

    public async Task<List<Asignacion>> ObtenerPorEmpleadoAsync(int empleadoId)
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Asignacion_ObtenerPorEmpleado", conexion);

        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@EmpleadoId", SqlDbType.Int).Value = empleadoId;

        await conexion.OpenAsync();
        await using var reader = await comando.ExecuteReaderAsync();

        var asignaciones = new List<Asignacion>();

        while (await reader.ReadAsync())
        {
            asignaciones.Add(MapearAsignacion(reader));
        }

        return asignaciones;
    }

    private static Asignacion MapearAsignacion(SqlDataReader reader)
    {
        return new Asignacion
        {
            AsignacionId = reader.GetInt64(reader.GetOrdinal("AsignacionId")),
            ActivoId = reader.GetInt32(reader.GetOrdinal("ActivoId")),
            CodigoActivo = reader.GetString(reader.GetOrdinal("CodigoActivo")),
            EmpleadoId = reader.GetInt32(reader.GetOrdinal("EmpleadoId")),
            NumeroEmpleado = reader.GetString(reader.GetOrdinal("NumeroEmpleado")),
            NombreEmpleado = reader.GetString(reader.GetOrdinal("NombreEmpleado")),
            FechaAsignacion = reader.GetDateTime(reader.GetOrdinal("FechaAsignacion")),
            FechaDevolucion = ObtenerFechaNullable(reader, "FechaDevolucion"),
            CondicionDevolucion = ObtenerStringNullable(reader, "CondicionDevolucion"),
            AsignadoPor = reader.GetInt32(reader.GetOrdinal("AsignadoPor")),
            DevueltoPor = ObtenerIntNullable(reader, "DevueltoPor"),
            Observaciones = ObtenerStringNullable(reader, "Observaciones")
        };
    }

    private static string? ObtenerStringNullable(SqlDataReader reader, string columna)
    {
        int ordinal = reader.GetOrdinal(columna);

        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    private static int? ObtenerIntNullable(SqlDataReader reader, string columna)
    {
        int ordinal = reader.GetOrdinal(columna);
        return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
    }

    private static DateTime? ObtenerFechaNullable(SqlDataReader reader, string columna)
    {
        int ordinal = reader.GetOrdinal(columna);
        return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
    }
}