namespace PIM_Data;

internal class Database
{
    private readonly static string connectionString =
       $"Server=localhost;" +
       $"Port=5432;" +
       $"User Id={UserCredentials.Name};" +
       $"Password={UserCredentials.Password};" +
       $"Database=postgres;" +
       $"Pooling=true;MinPoolSize=5;MaxPoolSize=50;";

    public static string ConnectionString { get { return connectionString; } }
}