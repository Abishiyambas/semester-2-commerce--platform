using System;
using System.Security.Cryptography;

namespace SHOP.Classes;

public class PasswordHasher
{
    public static string HashPassword(string password, out byte[] salt)
    {
        //Salt to make sure all passwords are hashed uniquely
        salt = new byte[16];
        RandomNumberGenerator.Fill(salt);
        
        // Use sha256 Algorithm to hash password - For now a string
        // Hashes the password 100.000 times
        var sha256 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            
        
        //Sets the string to a byte array - better for optimizing
        //From 256 bits to 32 bytes
        byte[] hash = sha256.GetBytes(32);
        
        //Returns a Base64 string instead of hash array - better for optimizing
        //Almost halfs length
        return Convert.ToBase64String(hash);
    }
    
    public static bool VerifyPassword(string password, byte[] salt, string storedHash)
    {
        // Hashes the new input
        var sha256 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        // Converts hashed password to bytes
        byte[] hash = sha256.GetBytes(32);
        
        // Converts to base64 string and verifies input
        return Convert.ToBase64String(hash) == storedHash;
    }
}