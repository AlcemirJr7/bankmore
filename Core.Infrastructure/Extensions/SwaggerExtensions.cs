using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Core.Infrastructure.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, string titleApi)
    {
        services.AddSwaggerGen(c =>
        {
            var provider = services.BuildServiceProvider()
                                   .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                c.SwaggerDoc(description.GroupName, new OpenApiInfo()
                {
                    Title = $"{titleApi} API v{description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    //definir configuracoes
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header Example: Bearer [token]",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}
                    }
                });
            }
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, string titleApi)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = string.Empty;
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.DocumentTitle = $"{titleApi} API {description.ApiVersion}";
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                        description.GroupName.ToUpperInvariant());
            }
        });

        return app;
    }
}
