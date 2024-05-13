using System.Security.Cryptography;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Ecommerce.WebAPI.src.Service
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password, out byte[] salt)
        {
            salt = GenerateSalt();
            return HashPassword(password, salt);
        }

        public bool VerifyPassword(string password, string passwordHash, byte[] salt)
        {
            string hashedPassword = HashPassword(password, salt);
            return hashedPassword == passwordHash;
        }

        private string HashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 600000,
                    numBytesRequested: 256 / 8
                )
            );
            return hashed;
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
