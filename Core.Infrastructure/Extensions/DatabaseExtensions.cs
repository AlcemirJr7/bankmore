using Core.Infrastructure.Database;
using Core.Infrastructure.Idempotencia.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        services.AddSingleton<IDbConnectionFactory>(_ =>
            new SqliteConnectionFactory(connectionString));

        services.AddScoped<IDatabaseInitializer, SqliteDatabaseInitializer>();
        services.AddTransient<IIdempotenciaRepository, IdempotenciaRepository>();

        return services;
    }

    public static void InitDatabase(this IApplicationBuilder builder, string command, IConfiguration configuration)
    {
        using (var scope = builder.ApplicationServices.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;

            initializer.InitDb(connectionString, command);
        }
    }
}
