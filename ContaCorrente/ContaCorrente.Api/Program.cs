using ContaCorrente.Infrastructure;
using ContaCorrente.Infrastructure.Database;
using Core.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApiVersionConfiguration();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfiguration("Conta Corrente");

app.InitDatabase(DbScripts.CreateTables, builder.Configuration);

app.UseMiddlewares();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.UseKafkaAsync();

app.Run();
