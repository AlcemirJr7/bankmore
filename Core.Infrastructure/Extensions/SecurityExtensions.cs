using Core.Infrastructure.Security.Auth;
using Core.Infrastructure.Security.Crypt;
using Core.Infrastructure.Security.Jwt;
using Core.Response;
using Core.Security.Auth;
using Core.Security.Crypt;
using Core.Security.Errors;
using Core.Security.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json;

namespace Core.Infrastructure.Extensions;

public static class SecurityExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILogin, Login>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IHasher, Hasher>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        services.AddHttpContextAccessor();
        services.AddTransient<AuthenticationDelegatingHandler>();
        services.AddTransient<ICryptService, CryptService>();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // 1. Bind e Valide JwtSettings
        services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection("JwtSettings"))
                .ValidateDataAnnotations() // Requer DataAnnotations no JwtSettings
                .Validate(settings =>
                {
                    if (string.IsNullOrWhiteSpace(settings.Secret))
                        return false;

                    if (settings.ExpirationMinutes <= 0)
                        return false;

                    return true;
                }, AuthErrors.JWT.InvalidSettings)
                .ValidateOnStart(); // Garante que a validação ocorra na inicialização da aplicação

        // 3. Configura a autenticação JWT Bearer
        services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
                        ?? throw new InvalidOperationException(AuthErrors.JWT.InvalidSettings);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Secret)
                        ),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1) // Ajuste para um valor mais tolerante, se apropriado
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Log.Error(context.Exception, AuthErrors.JWT.FailAuthentication);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Log.Information("Token validated for user: {User}", context.Principal?.Identity?.Name);
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse(); // Evita o comportamento padrão do desafio
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            var response = ApiResponse.Failure(AuthErrors.Login.Unauthorized);
                            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
                        }
                    };
                });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("HasContaCorrente", policy =>
                policy.RequireClaim("idContaCorrente"));
        });

        return services;
    }
}
