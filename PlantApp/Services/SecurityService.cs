using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PlantApp.Services
{
    public class SecurityService
    {
        public void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();

            salt = hmac.Key;

            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        } 
        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(storedHash);
        }

    }
}