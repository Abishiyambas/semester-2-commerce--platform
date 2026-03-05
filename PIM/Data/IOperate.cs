using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PIM_Data
{
    public interface IOperations
    {
        public Task<T?> SelectSingle<T> ( string tableName, string ColumnName, object? value, Func<NpgsqlDataReader, T> instantiateAction );
        public Task<T?> SelectSingle<T> ( string tableName, string ColumnName, object? value, string ColumnName2, object? value2, Func<NpgsqlDataReader, T> instantiateAction );

        public Task<List<T>> SelectAll<T> ( string tableName, Func<NpgsqlDataReader, T> instantiateAction );
        public Task<List<T>> SelectAll<T> ( string tableName, string columnName, object value, Func<NpgsqlDataReader, T> instantiateAction );
        
        public Task Update<T> ( string tableName, string whereColumnName, object isValue, T obj );
        
        public Task Delete ( string tableName, string ColumnName, object value );
        
        public Task Insert<T> ( string tableName, T obj );
        public Task<T> InsertSelect<T> ( string tableName, T obj, Func<NpgsqlDataReader, T> instantiateAction );
        public Task<int> Max ( string tableName, string ColumnName );

        public Task<int> Count ( string tableName, string ColumnName1, object? value1, string ColumnName2, object? value2 );
    }
}