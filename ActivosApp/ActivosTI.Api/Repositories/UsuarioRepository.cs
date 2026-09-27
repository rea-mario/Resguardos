using System.Data;
using ActivosTI.Api.Data;
using ActivosTI.Api.Models;
using Microsoft.Data.SqlClient;

namespace ActivosTI.Api.Repositories;

public class UsuarioRepository
{
    private readonly ConexionBD _conexionBD;

    public UsuarioRepository(ConexionBD conexionBD)
    {
        _conexionBD = conexionBD;
    }

    /// <summary>
    /// Busca un usuario por su nombre de usuario.
    /// </summary>
    public async Task<Usuario?> ObtenerPorUsuarioAsync(string usuario)
    {
        await using var conexion = _conexionBD.CrearConexion();

        await using var comando = new SqlCommand("SP_Usuario_Login", conexion);

        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@Usuario", SqlDbType.NVarChar, 100).Value = usuario;

        await conexion.OpenAsync();

        await using var reader = await comando.ExecuteReaderAsync(CommandBehavior.SingleRow);

        if (!await reader.ReadAsync())
        {
            return null;
        }
        
        int indiceIntentos = reader.GetOrdinal("IntentosFallidos");
        int indiceBloqueo = reader.GetOrdinal("BloqueadoHasta");

        return new Usuario
        {
            UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
            UsuarioNombre = reader.GetString(reader.GetOrdinal("Usuario")),
            HashContrasena = reader.GetString(reader.GetOrdinal("HashContrasena")),
            Rol = reader.GetString(reader.GetOrdinal("Rol")),
            Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
            IntentosFallidos = reader.IsDBNull(indiceIntentos)? 0 : reader.GetInt32(indiceIntentos),
            BloqueadoHasta = reader.IsDBNull(indiceBloqueo)? null: reader.GetDateTime(indiceBloqueo)
        };
    }

    public async Task<(int Intentos, DateTime? BloqueadoHasta)> RegistrarIntentoFallidoAsync(int usuarioId)
    {
        using var conexion = _conexionBD.CrearConexion();
        using var cmd = new SqlCommand( "SP_Usuario_RegistrarIntentoFallido", conexion);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuarioId;

        await conexion.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            int intentos = reader.GetInt32( reader.GetOrdinal("IntentosFallidos"));
            int indiceBloqueo = reader.GetOrdinal("BloqueadoHasta");
            DateTime? bloqueadoHasta = reader.IsDBNull(indiceBloqueo)? null: reader.GetDateTime(indiceBloqueo);

            return (intentos, bloqueadoHasta);
        }

        throw new InvalidOperationException("No fue posible registrar el intento fallido.");
    }

    public async Task RestablecerIntentosAsync(int usuarioId)
    {
        using var conexion = _conexionBD.CrearConexion();
        using var cmd = new SqlCommand("SP_Usuario_RestablecerIntentos", conexion);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = usuarioId;

        await conexion.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}