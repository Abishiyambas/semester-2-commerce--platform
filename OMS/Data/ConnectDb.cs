using Npgsql;
using System;

namespace OMS.Data;

static class ConnectDb {

    private static string _host = "localhost";  
    private static int _port = 5432;
    private static string _database = "oms";
    private static string? _user = "Arne"; //Environment.GetEnvironmentVariable("USER");
    private static string? _password = "admin";//Environment.GetEnvironmentVariable("PASSWORD");

    private static string _connectionString = $"Host={_host};Port={_port};Database={_database};Username={_user};Password={_password}";

    public static NpgsqlConnection GetNewConnection() {
      
      //DotNetEnv.Env.Load();
      Console.WriteLine(_connectionString);
      var connection = new NpgsqlConnection(_connectionString);
      try {
        connection.Open();
        Console.WriteLine("Connection established");
      }

      catch(Exception ex) {
        Console.WriteLine($"Connection failed {ex}");

      }
      return connection;
    }
    public static void Test()
    {
      NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
      try {
        conn.Open();
        Console.WriteLine("connection established");

      }
      catch(Exception ex) {
        Console.WriteLine($"Connection failed {ex}");

      }
    }
      

}
