namespace OMS.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using OMS.Data;
using System.Data;



public class ValidateUser
{
    //This method gets a database connection, then queries a command that tells how many employee that is with that username. If the number is more than 0 the method return true.
    public static bool Validater(string username, string password)
    {

        using (var conn = ConnectDb.GetNewConnection())
        {
            
            string query = "SELECT COUNT(*) FROM Employee WHERE Username = @Username AND Password = @Password";
            
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0;
            }
        }
    }
}
