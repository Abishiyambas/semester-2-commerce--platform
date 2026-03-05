using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PIM_Data
{
    public partial class DatabaseUtility
    {
        public async static Task ExecuteSqlFile ()
        {
            if ( PIM_Logic.AppEnvironment.IsTestEnvironment )
                return;
            try
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var projectRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;

                if ( projectRoot == null )
                {
                    throw new InvalidOperationException ( "Unable to determine the project root directory." );
                }

                var sqlFilePath = Path.Combine(projectRoot, "CreateDatabase.sql");

                string query = await File.ReadAllTextAsync(sqlFilePath);

                using ( var connection = new NpgsqlConnection ( Database.ConnectionString ) )
                {
                    await connection.OpenAsync ();

                    using ( var command = new NpgsqlCommand ( query, connection ) )
                    {
                        await command.ExecuteNonQueryAsync ();
                    }
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( $"An error occurred: {ex.Message}" );
            }
        }

        public async static Task<T?> ExecuteReaderSingleAsync<T> ( string commandText, Func<NpgsqlDataReader, T> instantiateAction, params NpgsqlParameter[] parameters )
        {
            try
            {
                using ( var connection = new NpgsqlConnection ( Database.ConnectionString ) )
                {
                    await connection.OpenAsync ();

                    using ( var command = new NpgsqlCommand ( commandText, connection ) )
                    {
                        command.Parameters.AddRange ( parameters );

                        using ( var reader = await command.ExecuteReaderAsync () )
                        {
                            if ( await reader.ReadAsync () )
                            {
                                return instantiateAction ( reader );
                            }
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                Console.Error.WriteLine ( $"Error executing query: {ex.Message}" );
            }

            return default;
        }

        public async static Task<List<T>> ExecuteReaderListAsync<T> ( string commandText, Func<NpgsqlDataReader, T> instantiateAction, params NpgsqlParameter[] parameters )
        {
            // List for holding the new objects of type <T>
            List<T> results = new List<T>();

            try
            {
                using ( var connection = new NpgsqlConnection ( Database.ConnectionString ) )
                {
                    await connection.OpenAsync ();

                    using ( var command = new NpgsqlCommand ( commandText, connection ) )
                    {

                        command.Parameters.AddRange ( parameters );

                        using var reader = await command.ExecuteReaderAsync ();
                        while ( await reader.ReadAsync () )
                            results.Add ( instantiateAction ( reader ) );
                    
                    }
                }
            }
            catch ( Exception ex )
            {
                Console.Error.WriteLine ( $"Error executing query: {ex.Message}" );
            }

            return results;
        }

        public async static Task ExecuteNonQueryAsync ( string commandText, params NpgsqlParameter[] parameters )
        {
            try
            {
                // Get a connection to the database 
                using ( var connection = new NpgsqlConnection ( Database.ConnectionString ) )
                {
                    await connection.OpenAsync ();

                    // Begin transaction
                    using ( var transaction = await connection.BeginTransactionAsync () )
                    {
                        try
                        {
                            using ( var command = new NpgsqlCommand ( commandText, connection, transaction ) )
                            {
                                command.Parameters.AddRange ( parameters );
                                await command.ExecuteNonQueryAsync ();
                            }

                            // Commit transaction
                            await transaction.CommitAsync ();
                        }
                        catch
                        {
                            // Rollback the transaction in case of an error
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                Console.Error.WriteLine ( $"Error executing: {ex.Message}" );
            }
        }

        public async static Task<T> ExecuteQueryAsync<T> ( string commandText, Func<NpgsqlDataReader, T> instantiateAction, params NpgsqlParameter[] parameters )
        {
            T? result = default;

            if ( !commandText.TrimEnd ().EndsWith ( ";" ) )
                commandText = commandText.TrimEnd () + " ";
            commandText += "RETURNING *";

            try
            {
                using ( var connection = new NpgsqlConnection ( Database.ConnectionString ) )
                {
                    await connection.OpenAsync ();

                    using ( var transaction = await connection.BeginTransactionAsync () )
                    {
                        try
                        {
                            using ( var command = new NpgsqlCommand ( commandText, connection ) )
                            {
                                command.Parameters.AddRange ( parameters );

                                using ( var reader = await command.ExecuteReaderAsync () )
                                {
                                    if ( await reader.ReadAsync () )
                                    {
                                        result = instantiateAction ( reader );
                                    }
                                }
                            }
                            // Commit transaction
                            await transaction.CommitAsync ();
                            return result;
                        }
                        catch
                        {
                            // Rollback the transaction in case of an error
                            await transaction.RollbackAsync ();
                            throw;
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                Console.Error.WriteLine ( $"Error executing: {ex.Message}" );
            }

            return default;
        }
    }
}