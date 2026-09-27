using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ActivosTI.Api.DTOs;
using ActivosTI.Api.Models;

namespace ActivosTI.Api.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public LoginResponse GenerarToken(Usuario usuario)
    {
        var jwt = _configuration.GetSection("Jwt");
        var minutos = int.Parse(jwt["ExpirationMinutes"] ?? "60");
        var expira = DateTime.Now.AddMinutes(minutos);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,usuario.UsuarioId.ToString()),
            new Claim( ClaimTypes.Name, usuario.UsuarioNombre),
            new Claim( ClaimTypes.Role, usuario.Rol)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecretKey"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: jwt["Issuer"], audience: jwt["Audience"], claims: claims, expires: expira, signingCredentials: credentials);

        return new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expira = expira,
            Usuario = usuario.UsuarioNombre,
            Rol = usuario.Rol
        };
    }
}