using System;
using System.Collections.Generic;
using Npgsql;
// ReSharper disable All

namespace DAM.Services;

public class PostGresContext
{
    private const string Host = "localhost";
    private const int Port = 5432;
    private const string Database = "dam";
    private const string Username = "testBruger";
    private const string Password = "123";

    private readonly NpgsqlConnection _connection;
    private readonly NpgsqlConnectionStringBuilder sb;
    

    public PostGresContext()
    {
        //string connectionString = $"Host={_host};Port={_port};Database={_database};Username={_username};Password={_password};";
        sb = new NpgsqlConnectionStringBuilder
        {
            IncludeErrorDetail = true,
            Host = Host,
            Port = Port,
            Username = Username,
            Password = Password,
            Database = Database
        };
        _connection = new NpgsqlConnection(sb.ConnectionString);
        _connection.Open();
    }

    public List<string> GetFileData(string name)
    {
        var cmd = new NpgsqlCommand($"SELECT product_id, name, type, path, upload_date FROM media WHERE name = '{name}'", _connection); // TODO Man skal kunne vælge mellem et enkelt billede eller flere billeder
        var reader = cmd.ExecuteReader();
        List<string> data = new List<string>();
        while (reader.Read())
        {
            data.Add((string)reader["product_id"]);
            data.Add((string)reader["name"]);
            data.Add((string)reader["type"]);
            data.Add((string)reader["upload_date"]);
        }
        
        reader.Close();
        
        return data;
    }

    public bool CreateFile(int productId, string name, string type, string path)
    {
    
        string date = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
        var sql = $"INSERT INTO media (product_id, name, type, path, upload_date) VALUES ('{productId}', '{name}', '{type}', '{path}', '{date}')";
        using var command = new NpgsqlCommand(sql, _connection);

    
        return command.ExecuteNonQuery() > 0; 
    }

    public bool UpdateFile(int productId, string name, string type, string path) // TODO do we need uuid here?
    {
        var sql = $"UPDATE media SET product_id='{productId}', name='{name}', type='{type}', path = '{path}' WHERE path = '{path}'";
        using var command = new NpgsqlCommand(sql, _connection);
        
        return command.ExecuteNonQuery() > 0;
    }

    public bool DeleteFile(string name)
    {
        var sql = $"DELETE FROM media WHERE name = '{name}'";
        using var command = new NpgsqlCommand(sql, _connection);
        
        return command.ExecuteNonQuery() > 0;
    }

    public bool addPictureConnection(int product_id)
    {

        return false;
    }
}