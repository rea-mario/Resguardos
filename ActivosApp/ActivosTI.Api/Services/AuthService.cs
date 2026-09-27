using ActivosTI.Api.DTOs;
using ActivosTI.Api.Helpers;
using ActivosTI.Api.Interfaces.Services;
using ActivosTI.Api.Repositories;

namespace ActivosTI.Api.Services;

public class AuthService:IAuthService
{
    private readonly UsuarioRepository _usuarioRepository;
    private readonly JwtHelper _jwtHelper;

    public AuthService(UsuarioRepository usuarioRepository, JwtHelper jwtHelper)
    {
        _usuarioRepository = usuarioRepository;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        // Buscar usuario en SQL Server
        var usuario = await _usuarioRepository.ObtenerPorUsuarioAsync(request.Usuario);

        // Usuario inexistente
        if (usuario == null || !usuario.Activo)
        {
            throw new UnauthorizedAccessException(
                "Usuario o contraseña incorrectos.");
        }

        // No permitir el login mientras siga bloqueado.
        if (usuario.BloqueadoHasta.HasValue &&
            usuario.BloqueadoHasta.Value > DateTime.Now)
        {
            throw new UnauthorizedAccessException(
                "La cuenta está bloqueada temporalmente. Intenta más tarde.");
        }


        // Usuario inactivo
        if (!usuario.Activo)
        {
            return null;
        }

        // Validar contraseña
        var contrasenaValida = BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.HashContrasena);

        if (!contrasenaValida)
        {
            var resultado = await _usuarioRepository.RegistrarIntentoFallidoAsync(usuario.UsuarioId);
            if (resultado.BloqueadoHasta.HasValue && resultado.BloqueadoHasta.Value > DateTime.Now)
            {
                throw new UnauthorizedAccessException(
                    "Cuenta bloqueada por 15 minutos debido a intentos fallidos.");
            }

            throw new UnauthorizedAccessException( "Usuario o contraseña incorrectos.");

        }

        await _usuarioRepository.RestablecerIntentosAsync(usuario.UsuarioId);

        // Generar JWT
        return _jwtHelper.GenerarToken(usuario);
    }
}