using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIM_Data
{
    public partial class DatabaseUtility : IOperations // All methods here make use of the methods found in DatabaseUtiliy
    {
        public async Task<T?> SelectSingle<T> ( string tableName, string ColumnName, object? value, Func<NpgsqlDataReader, T> instantiateAction )
        {
            string query = $"SELECT * FROM {tableName} WHERE {ColumnName} = @value";

            var parameters = new[] {new NpgsqlParameter( "@value", value ) };

            return await ExecuteReaderSingleAsync ( query, instantiateAction, parameters );
        }
        public async Task<T?> SelectSingle<T> ( string tableName, string ColumnName, object? value, string ColumnName2, object? value2, Func<NpgsqlDataReader, T> instantiateAction )
        {
            string query = $"SELECT * FROM {tableName} WHERE {ColumnName} = @value AND {ColumnName2} = @value2";

            var parameters = new[] { 
                new NpgsqlParameter( "@value", value ), 
                new NpgsqlParameter( "@value2", value2 ) };

            return await ExecuteReaderSingleAsync ( query, instantiateAction, parameters );
        }


        public async Task<List<T>> SelectAll<T> ( string tableName, Func<NpgsqlDataReader, T> instantiateAction )
        {
            string query = $"SELECT * FROM {tableName}";
            return await ExecuteReaderListAsync ( query, instantiateAction );
        }

        // Select all based on a parameter
        public async Task<List<T>> SelectAll<T> ( string tableName, string columnName, object value, Func<NpgsqlDataReader, T> instantiateAction )
        {
            string query = $"SELECT * FROM {tableName} WHERE {columnName} = @value";
            
            var parameters = new [] { new NpgsqlParameter( "@value", value ) };

            return await ExecuteReaderListAsync ( query, instantiateAction, parameters );
        }

        public async Task Delete ( string tableName, string ColumnName, object value )
        {
            string query = $"DELETE FROM {tableName} WHERE {ColumnName} = @value";
            var parameters = new[] { new NpgsqlParameter( "@value", value ) };
            await ExecuteNonQueryAsync ( query, parameters );
        }

        public async Task Insert<T> ( string tableName, T obj )
        {
            // Get an array of all properties the class "T" has. Hence all property names should 
            // reflect column names in the tables
            var properties = typeof(T).GetProperties();
            // Skip the first value, since this is the id, which is a serial in the database
            properties = properties.Skip ( 1 ).ToArray ();
            // Since the property names reflect the column names, we can string them together to create 
            // an unique query for all types
            var columnNames = string.Join(", ", properties.Select(p => p.Name.ToLower())); 
            var parameterNames = string.Join(", ", properties.Select(p => $"@{p.Name.ToLower()}"));
            // All this together creates the query
            string query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames})"; 
            // The npgsqlParameters are setup based upon the initial properties array
            var parameters = properties.Select( 
                property => new NpgsqlParameter(
                    $"@{property.Name.ToLower()}",          // Parameter name
                    property.GetValue(obj) ?? DBNull.Value) // Property value
                ).ToArray();

            await ExecuteNonQueryAsync ( query, parameters );
        }

        // Insert then select the same item
        public async Task<T> InsertSelect<T> ( string tableName, T obj, Func<NpgsqlDataReader, T> instantiateAction )
        {
            
            var properties = typeof(T).GetProperties();
            properties = properties.Skip ( 1 ).ToArray ();
            var columnNames = string.Join(", ", properties.Select(p => p.Name.ToLower()));
            var parameterNames = string.Join(", ", properties.Select(p => $"@{p.Name.ToLower()}"));
            string query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames})";
            var parameters = properties.Select(
                property => new NpgsqlParameter(
                    $"@{property.Name.ToLower()}",          // Parameter name
                    property.GetValue(obj) ?? DBNull.Value) // Property value
                ).ToArray();

            return await ExecuteQueryAsync ( query, instantiateAction, parameters );
        }

        public async Task Update<T> ( string tableName, string whereColumnName, object isValue, T obj )  // Here the same principles apply as the method above
        {
            var properties = typeof(T).GetProperties();

            var updateValues = string.Join(", ", properties.Select(p => $"{p.Name.ToLower()} = @{p.Name.ToLower()}"));

            string query = $"UPDATE {tableName} SET {updateValues} WHERE {whereColumnName} = {isValue}";

            var parameters = properties.Select(
                p => new NpgsqlParameter(
                    $"@{p.Name.ToLower()}", p.GetValue(obj) ?? DBNull.Value))
                .ToArray();

            await ExecuteNonQueryAsync ( query, parameters );
        }

        public async Task<int> Count ( string tableName, string ColumnName1, int? value1 )
        {
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {ColumnName1} = @value1";

            var parameters = new[] {
                new NpgsqlParameter( "@value1", value1 ),
            };

            return await ExecuteReaderSingleAsync ( query, reader => reader.GetInt32 ( 0 ), parameters );
        }

        public async Task<int> Count( string tableName, string ColumnName1, object? value1, string ColumnName2, object? value2 )
        {
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {ColumnName1} = @value1 AND {ColumnName2} = @value2";

            var parameters = new[] {
                new NpgsqlParameter( "@value1", value1 ), 
                new NpgsqlParameter( "@value2", value2 )
            };

            return await ExecuteReaderSingleAsync ( query, reader => reader.GetInt32(0), parameters );
        }
        


        // Get the maximum value from a column 
        public async Task<int> Max( string tableName, string ColumnName)
        {
            string query = $"SELECT MAX({ColumnName}) FROM {tableName}";

            return await ExecuteReaderSingleAsync ( query, reader => reader.GetInt32 ( 0 ) );
        }
    }
}