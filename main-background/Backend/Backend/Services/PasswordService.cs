using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

    public static class PasswordService
    {
        public static string HashPassword(string Password)
        {

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            byte[] hash =KeyDerivation.Pbkdf2(
            password: Password!,
            prf: KeyDerivationPrf.HMACSHA256,
            salt: salt,
            iterationCount: 100000,
            numBytesRequested: 256 / 8);
             byte[] hashBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);
            
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string Password, string passwordHash)
        {
            byte[] hsh = Convert.FromBase64String(passwordHash);
            byte[] salt = new byte[16];
            Array.Copy(hsh, 0, salt, 0, salt.Length);
            byte[] originalHsh = new byte[hsh.Length - salt.Length];
            Array.Copy(hsh, salt.Length, originalHsh, 0, originalHsh.Length);

            byte[] newHsh = KeyDerivation.Pbkdf2(
            password: Password!,
            prf: KeyDerivationPrf.HMACSHA256,
            salt: salt,
            iterationCount: 100000,
            numBytesRequested: 256 / 8);
            return CryptographicOperations.FixedTimeEquals(originalHsh, newHsh);
        }
    }
