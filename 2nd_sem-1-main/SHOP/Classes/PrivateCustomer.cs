using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using SHOP.Classes;

namespace SHOP;

public class PrivateCustomer : User
{
    public DateTime dateTime { get; set; }
    private readonly Credentials.Credentials _credentials = new();
    public string Name { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public static event Action<PrivateCustomer> OnLoginSuccess;
    public override string DisplayName => Name;
    
    
    public PrivateCustomer(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public PrivateCustomer() { }

    public static PrivateCustomer LoggedInCustomer { get; private set; }

    public static PrivateCustomer LogIn(string email, string password)
    { 
        const string query = @"
            SELECT 
                id, 
                name,
                email, 
                password,
                password_salt AS PasswordSalt,
                createdon, 
                lastlogin, 
                address, 
                zip, 
                city, 
                country, 
                telephone
            FROM customers
            WHERE email = @Email
            LIMIT 1;";
        var credentials = new Credentials.Credentials();
        using var con = new NpgsqlConnection(credentials.Database);

        try
        {
            var user = con.QueryFirstOrDefault<PrivateCustomer>(query, new { Email = email});
            
            if (user != null)
            {
                //Fetches salt from database to ensure password is checked correctly
                byte[] saltBytes = Convert.FromBase64String(user.PasswordSalt);
                //Hashes the input using the same salt as register, then checks if they are identical
                bool isValid = PasswordHasher.VerifyPassword(password, saltBytes, user.Password);
                if (isValid)
                {
                    LoggedInCustomer = user;

                    string updateLastLogin = "UPDATE customers SET lastlogin = @Now WHERE id = @Id";
                    con.Execute(updateLastLogin, new { Now = DateTime.Now, Id = user.Id });
                    OnLoginSuccess?.Invoke(user);
                    Console.WriteLine("Logged in as: " + user.Name);   
                }
            }
            else
            {
                Console.WriteLine("Login failed.");
            }

            return user;
        }
        catch
        {
            Console.WriteLine("An error occurred during login.");
            return null;
        }
    }

    // Method for Register 
    public PrivateCustomer? Register(string name, string email, string password, string address, string zip, string city, string country, string telephone)
    {
        const string insertQuery = @"
            INSERT INTO customers (name, email, password, address, zip, city, country, telephone, createdon, lastlogin)
            VALUES (@Name, @Email, @Password, @Address, @Zip, @City, @Country, @Telephone, @CreatedOn, @LastLogin)
            RETURNING id";

        using var con = new NpgsqlConnection(_credentials.Database);
        con.Open();

        // Insert and get new ID
        try
        {
            
            string hashedPassword = PasswordHasher.HashPassword(password, out byte[] salt);
            //Converts the salt to string so it can be stored in database - storing byte[] is more complex
            string saltBase64 = Convert.ToBase64String(salt);
            
            string insertWithSaltQuery = @"
            INSERT INTO customers (name, email, password, password_salt, address, zip, city, country, telephone, createdon, lastlogin)
            VALUES (@Name, @Email, @Password, @Salt, @Address, @Zip, @City, @Country, @Telephone, @CreatedOn, @LastLogin)
            RETURNING id";
            //Stores password as hashed
            //Stores salt, this needs to be retrieved for login
            int newId = con.ExecuteScalar<int>(insertWithSaltQuery, new
            {
                Name = name,
                Email = email,
                Password = hashedPassword,
                Salt = saltBase64,
                Address = address,
                Zip = zip,
                City = city,
                Country = country,
                Telephone = telephone,
                CreatedOn = DateTime.Now,
                LastLogin = DateTime.Now
            });

            this.Id = newId;
            Console.WriteLine("User registered on: " + DateTime.Now);
            return this;
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred during registration: " + e.Message);
            return null;
        }
    }
    public PrivateCustomer? RegisterNoLogin(string name, string email, string address, string zip, string city, string country, string telephone)
    {
        const string insertQuery = @"
        INSERT INTO customers (name, email, address, zip, city, country, telephone, createdon, lastlogin)
        VALUES (@Name, @Email, @Address, @Zip, @City, @Country, @Telephone, @CreatedOn, @LastLogin)
        RETURNING id";

        using var con = new NpgsqlConnection(_credentials.Database);
        con.Open();

        try
        {
            int newId = con.ExecuteScalar<int>(insertQuery, new
            {
                Name = name,
                Email = email,
                Address = address,
                Zip = zip,
                City = city,
                Country = country,
                Telephone = telephone,
                CreatedOn = DateTime.Now,
                LastLogin = DateTime.Now
            });

            this.Id = newId;
            Console.WriteLine("User registered on: " + DateTime.Now);
            return this;
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred during registration: " + e.Message);
            return null;
        }
    }

}




