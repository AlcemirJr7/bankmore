namespace Core.Infrastructure.Database;

public interface IDatabaseInitializer
{
    public void InitDb(string connectionString, string command);
}
