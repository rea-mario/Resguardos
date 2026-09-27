using Microsoft.Data.SqlClient;
using System.Data;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Data;

namespace ActivosTI.Api.Repositories;

public class ProveedorRepository
{
    private readonly ConexionBD _conexionBD;
    public ProveedorRepository(ConexionBD conexionBD)
    {
        _conexionBD = conexionBD;
    }

    public async Task<int> CrearAsync(CrearProveedorRequest request)
    {
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Proveedores_Crear", conexion);

        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 200).Value = request.Nombre;
        comando.Parameters.Add("@NombreContacto", SqlDbType.NVarChar, 150).Value = (object?)request.NombreContacto ?? DBNull.Value;
        comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 150).Value = (object?)request.Correo ?? DBNull.Value;
        comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 50).Value = (object?)request.Telefono ?? DBNull.Value;
        comando.Parameters.Add("@ProporcionaCompra", SqlDbType.Bit).Value = request.ProporcionaCompra;
        comando.Parameters.Add("@ProporcionaRenta", SqlDbType.Bit).Value = request.ProporcionaRenta;
        comando.Parameters.Add("@ProporcionaMantenimiento", SqlDbType.Bit).Value = request.ProporcionaMantenimiento;
        comando.Parameters.Add("@RFC", SqlDbType.NVarChar, 20).Value = request.RFC;
        await conexion.OpenAsync();
        var result = await comando.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<List<ProveedorDto>> ConsultarAsync(int? proveedorId = null, string? nombre = null, string? rfc = null, bool? activo = null, bool? proporcionaCompra = null, bool? proporcionaRenta = null, bool? proporcionaMantenimiento = null)
    {
        var proveedores = new List<ProveedorDto>();
        await using var conexion = _conexionBD.CrearConexion();
        await using var comando = new SqlCommand("SP_Proveedores_Consultar", conexion);

        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@ProveedorId", SqlDbType.Int).Value = (object?)proveedorId ?? DBNull.Value;
        comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 200).Value = (object?)nombre ?? DBNull.Value;
        comando.Parameters.Add("@RFC", SqlDbType.NVarChar, 20).Value = (object?)rfc ?? DBNull.Value;
        comando.Parameters.Add("@Activo", SqlDbType.Bit).Value = (object?)activo ?? DBNull.Value;
        comando.Parameters.Add("@ProporcionaCompra", SqlDbType.Bit).Value = (object?)proporcionaCompra ?? DBNull.Value;
        comando.Parameters.Add("@ProporcionaRenta", SqlDbType.Bit).Value = (object?)proporcionaRenta ?? DBNull.Value;
        comando.Parameters.Add("@ProporcionaMantenimiento", SqlDbType.Bit).Value = (object?)proporcionaMantenimiento ?? DBNull.Value;
        await conexion.OpenAsync();
        await using var reader = await comando.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            proveedores.Add(new ProveedorDto
            {
                ProveedorId = reader.GetInt32(reader.GetOrdinal("ProveedorId")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                NombreContacto = reader.IsDBNull(reader.GetOrdinal("NombreContacto")) ? null : reader.GetString(reader.GetOrdinal("NombreContacto")),
                Correo = reader.IsDBNull(reader.GetOrdinal("Correo")) ? null : reader.GetString(reader.GetOrdinal("Correo")),
                Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                ProporcionaCompra = reader.GetBoolean(reader.GetOrdinal("ProporcionaCompra")),
                ProporcionaRenta = reader.GetBoolean(reader.GetOrdinal("ProporcionaRenta")),
                ProporcionaMantenimiento = reader.GetBoolean(reader.GetOrdinal("ProporcionaMantenimiento")),
                RFC = reader.GetString(reader.GetOrdinal("RFC")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
            });
        }
        return proveedores;
    }
}
