using Microsoft.Data.SqlClient;

namespace ActivosTI.Api.Data;

public class ConexionBD
{
    private readonly string _connectionString;

    public ConexionBD(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConexionSQL") ?? throw new InvalidOperationException("No se configuró la conexión ConexionSQL.");
    }

    public SqlConnection CrearConexion()
    {
        return new SqlConnection(_connectionString);
    }
}