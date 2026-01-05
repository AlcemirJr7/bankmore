using Dapper;
using Microsoft.Data.Sqlite;
using Serilog;

namespace Core.Infrastructure.Database;

public class SqliteDatabaseInitializer : IDatabaseInitializer
{
    public void InitDb(string connectionString, string command)
    {
        try
        {
            var builder = new SqliteConnectionStringBuilder(connectionString);

            var databaseFile = builder.DataSource;

            if (!string.IsNullOrWhiteSpace(databaseFile))
            {
                var folder = Path.GetDirectoryName(databaseFile);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder!);
            }

            using var connection = new SqliteConnection(connectionString);

            connection.Open();

            connection.Execute(command);

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao inicializar banco de dados.");
            throw;
        }
    }
}
