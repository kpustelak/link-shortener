using System.Security.Cryptography;
using LinkShortener.API.Interface;

namespace LinkShortener.API.Services;

public class PasswordService : IPasswordService
{
    
    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(PasswordHasher.SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            PasswordHasher.Iterations,
            PasswordHasher.Algorithm,
            PasswordHasher.HashSize);
        return $"{Convert.ToHexString(salt)}-{Convert.ToHexString(hash)}";

    }

    public bool VerifyHashedPassword(string hashedPassword, string password)
    {
        string[] parts =  hashedPassword.Split("-");
        if (parts.Length != 2)
        {
            return false;
        }

        try
        {
            byte[] salt = Convert.FromHexString(parts[0]);
            byte[] hash = Convert.FromHexString(parts[1]);
            
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                PasswordHasher.Iterations,
                PasswordHasher.Algorithm,
                PasswordHasher.HashSize);
            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
        catch (Exception ex)
        {
            throw new Exception("Invalid password", ex);
        }
        
    }
    
    private class PasswordHasher
    {
        public const int SaltSize = 16;
        public const int HashSize = 32;
        public const int Iterations = 500_000;
        public static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
    }
}