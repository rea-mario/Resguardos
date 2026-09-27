using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace ActivosTI.Api.Middleware;

public class ManejoErroresMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ManejoErroresMiddleware> _logger;

    public ManejoErroresMiddleware(
        RequestDelegate next,
        ILogger<ManejoErroresMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Error SQL {Number}: {Message}",
                ex.Number,
                ex.Message);

            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            int statusCode;
            string mensaje;

            switch (ex.Number)
            {
                 case 50001:
                 case 50010:
                 case 50020:
                 case 50021:
                 case 50022: 
                 case 50026:  
                 case 50029:
                 case 50032:
                    statusCode = (int)HttpStatusCode.Conflict;
                    mensaje = ex.Message;
                    break;

                case 2627:
                case 2601:
                    statusCode = (int)HttpStatusCode.Conflict;
                    mensaje = "El registro ya existe.";
                    break;

                case 547:
                    statusCode = (int)HttpStatusCode.Conflict;
                    mensaje = "No se puede realizar la operación debido a una relación existente.";
                    break;

                case -2:
                    statusCode = (int)HttpStatusCode.GatewayTimeout;
                    mensaje = "La base de datos tardó demasiado en responder.";
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    mensaje = "Ocurrió un error al acceder a la base de datos.";
                    break;
            }

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new
                {
                    mensaje
                }));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al procesar la solicitud.");

            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();
            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new
                {
                    mensaje = ex.Message
                }));
        }
    }
}