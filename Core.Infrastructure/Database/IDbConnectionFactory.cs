using System.Data;

namespace Core.Infrastructure.Database;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
