using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AngularAuthInfrastructure.Helpers;

public static class HashPasswordExtensions
{
    public static string HashPasswordPbkdf2(this string password)
    {

        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }


        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));


        return $"{Convert.ToBase64String(salt)}|{hashedPassword}";
    }

    public static bool VerifyPassword(this string password, string storedHash)
    {

        var parts = storedHash.Split('|');
        if (parts.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        string storedHashedPassword = parts[1];


        string hashedInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));


        return hashedInput == storedHashedPassword;
    }
}
