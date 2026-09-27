using System.Text;
using Microsoft.IdentityModel.Tokens;
using ActivosTI.Api.Data;
using ActivosTI.Api.Helpers;
using ActivosTI.Api.Middleware;
using ActivosTI.Api.Repositories;
using ActivosTI.Api.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.RateLimiting;
using ActivosTI.Api.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Controladores
builder.Services.AddControllers();

// Swagger con autenticación Bearer
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Gestión de Activos TI",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Ingresa el token JWT."
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("LoginLimit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey:
                httpContext.Connection.RemoteIpAddress?.ToString()
                ?? "ip-desconocida",

            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.Headers["Retry-After"] = "60";

        await context.HttpContext.Response.WriteAsJsonAsync(
            new
            {
                mensaje = "Demasiados intentos de inicio de sesión. Intenta nuevamente más tarde."
            },
            cancellationToken);
    };
});

// Conexión y servicios
builder.Services.AddSingleton<ConexionBD>();
builder.Services.AddSingleton<JwtHelper>();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<IAuthService,AuthService>();

builder.Services.AddScoped<ActivoRepository>();
builder.Services.AddScoped<IActivoService,ActivoService>();

builder.Services.AddScoped<EmpleadoRepository>();
builder.Services.AddScoped<IEmpleadoService,EmpleadoService>();

builder.Services.AddScoped<AsignacionRepository>();
builder.Services.AddScoped<IAsignacionService,AsignacionService>();

builder.Services.AddScoped<ProveedorRepository>(); 
builder.Services.AddScoped<IProveedorService, ProveedorService>();

// JWT
var jwt = builder.Configuration.GetSection("Jwt");

var secretKey = jwt["SecretKey"]
    ?? throw new InvalidOperationException(
        "No se configuró Jwt:SecretKey.");

if (Encoding.UTF8.GetByteCount(secretKey) < 32)
{
    throw new InvalidOperationException(
        "La clave JWT debe tener al menos 32 bytes.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwt["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwt["Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware
app.UseMiddleware<ManejoErroresMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();