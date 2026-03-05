using System;
using Dapper;
using Npgsql;
namespace SHOP.Classes;

public abstract class User
{
    
    public int Id { get; protected set; }
    public abstract string DisplayName { get; }
    public string Email { get; protected set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastLogin { get; set; }
    
    public string Address { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Telephone { get; set; }
    public string? Password { get; set; }
    
    private byte[]? _salt;
    
    public void SetPassword(string password)
    {
        Password = PasswordHasher.HashPassword(password, out var salt);
        _salt = salt;
    }

    public bool VerifyPassword(string input)
    {   
        return PasswordHasher.VerifyPassword(input, _salt, Password);
    }
    
}