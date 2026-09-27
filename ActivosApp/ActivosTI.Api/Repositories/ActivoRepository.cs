using System.Data;
using ActivosTI.Api.Data;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Models;
using Microsoft.Data.SqlClient;

namespace ActivosTI.Api.Repositories;

public class ActivoRepository
{
    private readonly ConexionBD _conexionBD;

    public ActivoRepository(ConexionBD conexionBD)
    {
        _conexionBD = conexionBD;
    }

    public async Task<int> CrearAsync(
        CrearActivoRequest request)
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Activos_Crear", conexion);

        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@CodigoActivo", SqlDbType.NVarChar, 50).Value = request.CodigoActivo;
        comando.Parameters.Add("@NumeroSerie", SqlDbType.NVarChar, 100).Value = (object?)request.NumeroSerie ?? DBNull.Value;
        comando.Parameters.Add("@Categoria", SqlDbType.NVarChar, 100).Value = request.Categoria;
        comando.Parameters.Add("@Marca", SqlDbType.NVarChar, 100).Value = (object?)request.Marca ?? DBNull.Value;
        comando.Parameters.Add("@Modelo", SqlDbType.NVarChar, 100).Value = (object?)request.Modelo ?? DBNull.Value;
        comando.Parameters.Add("@TipoPropiedad", SqlDbType.NVarChar, 30).Value = request.TipoPropiedad;
        comando.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = (object?)request.ProveedorId ?? DBNull.Value;
        comando.Parameters.Add("@Estado", SqlDbType.NVarChar, 30).Value = (object?)request.Estado ?? DBNull.Value;
        comando.Parameters.Add("@UbicacionActual", SqlDbType.NVarChar, 200).Value = (object?)request.UbicacionActual ?? DBNull.Value;
        comando.Parameters.Add("@FechaCompra", SqlDbType.Date).Value = (object?)request.FechaCompra ?? DBNull.Value;
        comando.Parameters.Add("@FechaFinRenta", SqlDbType.Date).Value = (object?)request.FechaFinRenta ?? DBNull.Value;
        await conexion.OpenAsync();

        var resultado = await comando.ExecuteScalarAsync();

        return Convert.ToInt32(resultado);
    }


    // =========================================================
    // OBTENER ACTIVO POR ID
    // =========================================================

    public async Task<Activo?> ObtenerPorIdAsync(int activoId)
    {
        await using var conexion = _conexionBD.CrearConexion();

        await using var comando = new SqlCommand("SP_Activo_ObtenerPorId", conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@ActivoId", SqlDbType.Int).Value = activoId;

        await conexion.OpenAsync();

        await using var reader = await comando.ExecuteReaderAsync(CommandBehavior.SingleRow);

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return MapearActivo(reader);
    }


    // =========================================================
    // OBTENER ACTIVOS PAGINADOS
    // =========================================================

    public async Task<List<Activo>> ObtenerPaginadoAsync(string? search, string? estado, string? categoria, int pagina, int registrosPorPagina)
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Activo_ObtenerPaginado", conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@Search", SqlDbType.NVarChar, 200).Value = (object?)search ?? DBNull.Value;

        comando.Parameters.Add("@Estado", SqlDbType.NVarChar, 30).Value = (object?)estado ?? DBNull.Value;

        comando.Parameters.Add("@Categoria", SqlDbType.NVarChar, 100).Value = (object?)categoria ?? DBNull.Value;

        comando.Parameters.Add("@Pagina", SqlDbType.Int).Value = pagina;
        comando.Parameters.Add("@RegistrosPorPagina", SqlDbType.Int).Value = registrosPorPagina;

        await conexion.OpenAsync();

        await using var reader = await comando.ExecuteReaderAsync();

        var activos = new List<Activo>();

        while (await reader.ReadAsync())
        {
            activos.Add(MapearActivo(reader));
        }

        return activos;
    }


    // =========================================================
    // ACTUALIZAR ACTIVO
    // =========================================================

    public async Task ActualizarAsync(int activoId, CrearActivoRequest request, int usuarioId)
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Activo_Actualizar", conexion);

        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@ActivoId", SqlDbType.Int).Value = activoId;
        comando.Parameters.Add("@CodigoActivo", SqlDbType.NVarChar, 50).Value = request.CodigoActivo;
        comando.Parameters.Add("@NumeroSerie", SqlDbType.NVarChar, 100).Value = (object?)request.NumeroSerie ?? DBNull.Value;
        comando.Parameters.Add("@Categoria", SqlDbType.NVarChar, 100).Value = request.Categoria;
        comando.Parameters.Add("@Marca", SqlDbType.NVarChar, 100).Value = (object?)request.Marca ?? DBNull.Value;
        comando.Parameters.Add("@Modelo", SqlDbType.NVarChar, 100).Value = (object?)request.Modelo ?? DBNull.Value;
        comando.Parameters.Add("@TipoPropiedad", SqlDbType.NVarChar, 30).Value = request.TipoPropiedad;
        comando.Parameters.Add("@ProveedorId", SqlDbType.Int).Value = (object?)request.ProveedorId ?? DBNull.Value;
        comando.Parameters.Add("@UbicacionActual", SqlDbType.NVarChar, 200).Value = (object?)request.UbicacionActual ?? DBNull.Value;
        comando.Parameters.Add("@FechaCompra", SqlDbType.Date).Value = (object?)request.FechaCompra ?? DBNull.Value;
        comando.Parameters.Add("@FechaFinRenta", SqlDbType.Date).Value = (object?)request.FechaFinRenta ?? DBNull.Value;
        comando.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuarioId;

        await conexion.OpenAsync();

        await comando.ExecuteNonQueryAsync();
    }

    /// <summary> /// Consultar activos /// </summary> 
    public async Task<IEnumerable<Activo>> ConsultarAsync(int? id = null, string? codigoActivo = null, string? categoria = null, string? estado = null, string? tipoPropiedad = null, int? idProveedor = null)
    {
        var activos = new List<Activo>();
        await using var conexion = _conexionBD.CrearConexion();
        await using var command = new SqlCommand("SP_Activos_Consultar", conexion);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Id", SqlDbType.Int).Value = (object?)id ?? DBNull.Value;
        command.Parameters.Add("@CodigoActivo", SqlDbType.NVarChar, 50).Value = (object?)codigoActivo ?? DBNull.Value;
        command.Parameters.Add("@Categoria", SqlDbType.NVarChar, 100).Value = (object?)categoria ?? DBNull.Value;
        command.Parameters.Add("@Estado", SqlDbType.NVarChar, 30).Value = (object?)estado ?? DBNull.Value;
        command.Parameters.Add("@TipoPropiedad", SqlDbType.NVarChar, 30).Value = (object?)tipoPropiedad ?? DBNull.Value;
        command.Parameters.Add("@IdProveedor", SqlDbType.Int).Value = (object?)idProveedor ?? DBNull.Value;
        await conexion.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            activos.Add(
                new Activo
                {
                    ActivoId = reader.GetInt32(reader.GetOrdinal("ActivoId")),
                    CodigoActivo = reader.GetString(reader.GetOrdinal("AssetCode")),
                    NumeroSerie = reader.IsDBNull(reader.GetOrdinal("SerialNumber")) ? null : reader.GetString(reader.GetOrdinal("SerialNumber")),
                    Categoria = reader.GetString(reader.GetOrdinal("Category")),
                    Marca = reader.IsDBNull(reader.GetOrdinal("Brand")) ? null : reader.GetString(reader.GetOrdinal("Brand")),
                    Modelo = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader.GetString(reader.GetOrdinal("Model")),
                    TipoPropiedad = reader.GetString(reader.GetOrdinal("OwnershipType")),
                    ProveedorId = reader.IsDBNull(reader.GetOrdinal("SupplierId")) ? null : reader.GetInt32(reader.GetOrdinal("SupplierId")),
                    // Proveedor = reader.IsDBNull(reader.GetOrdinal("Proveedor")) ? null : reader.GetString(reader.GetOrdinal("Proveedor")),
                    Estado = reader.GetString(reader.GetOrdinal("Status")),
                    UbicacionActual = reader.IsDBNull(reader.GetOrdinal("CurrentLocation")) ? null : reader.GetString(reader.GetOrdinal("CurrentLocation")),
                    FechaCompra = reader.IsDBNull(reader.GetOrdinal("PurchaseDate")) ? null : reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                    FechaFinRenta = reader.IsDBNull(reader.GetOrdinal("RentalEndDate")) ? null : reader.GetDateTime(reader.GetOrdinal("RentalEndDate")),
                    // FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                    //FechaActualizacion = reader.IsDBNull(reader.GetOrdinal("FechaActualizacion")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaActualizacion"))
                });
        }
        return activos;
    }

    // =========================================================
    // MAPEO
    // =========================================================

    private static Activo MapearActivo(
        SqlDataReader reader)
    {
        return new Activo
        {
            ActivoId = reader.GetInt32(reader.GetOrdinal("ActivoId")),
            CodigoActivo = reader.GetString(reader.GetOrdinal("CodigoActivo")),
            NumeroSerie = ObtenerStringNullable(reader, "NumeroSerie"),
            Categoria = reader.GetString(reader.GetOrdinal("Categoria")),
            Marca = ObtenerStringNullable(reader, "Marca"),
            Modelo = ObtenerStringNullable(reader, "Modelo"),
            TipoPropiedad = reader.GetString(reader.GetOrdinal("TipoPropiedad")),
            ProveedorId = ObtenerIntNullable(reader, "ProveedorId"),
            Estado = reader.GetString(reader.GetOrdinal("Estado")),
            UbicacionActual = ObtenerStringNullable(reader, "UbicacionActual"),
            FechaCompra = ObtenerDateTimeNullable(reader, "FechaCompra"),
            FechaFinRenta = ObtenerDateTimeNullable(reader, "FechaFinRenta")
        };
    }

    private static string? ObtenerStringNullable(SqlDataReader reader, string columna)
    {
        var ordinal = reader.GetOrdinal(columna);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    private static int? ObtenerIntNullable(SqlDataReader reader, string columna)
    {
        var ordinal = reader.GetOrdinal(columna);

        return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
    }


    private static DateTime? ObtenerDateTimeNullable(SqlDataReader reader, string columna)
    {
        var ordinal = reader.GetOrdinal(columna);

        return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
    }
}