using Microsoft.Data.Sqlite;
using System.Data;

namespace Core.Infrastructure.Database;

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var conn = new SqliteConnection(_connectionString);
        return conn;
    }
}
