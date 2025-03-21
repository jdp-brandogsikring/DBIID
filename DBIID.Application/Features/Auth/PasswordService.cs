using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBIID.Application.Features.Auth
{
    public class PasswordService : IPasswordService
    {
        // Konstanter til saltstørrelse, nøglestørrelse og antallet af iterationer
        private const int SaltSize = 16; // 16 bytes = 128 bit
        private const int KeySize = 32;  // 32 bytes = 256 bit
        private const int Iterations = 10000;

        public string HashPassword(string password)
        {
            // Generer et tilfældigt salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Anvend PBKDF2 til at generere en hash baseret på password og salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(KeySize);

                // Kombinér salt og hash i én byte-array
                byte[] hashBytes = new byte[SaltSize + KeySize];
                Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
                Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, KeySize);

                // Returnér kombineret salt og hash som en Base64-streng
                return Convert.ToBase64String(hashBytes);
            }
        }

        public bool ValidatePassword(string password, string hashedPassword)
        {
            // Dekodér den gemte Base64-streng til en byte-array
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            if (hashBytes.Length != SaltSize + KeySize)
                return false; // Hvis formatet ikke stemmer overens, er den gemte hash ugyldig

            // Udtræk salt fra den gemte hash
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

            // Genberegn hash for det angivne password ved brug af den udtrukne salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(KeySize);
                // Sammenlign den genberegnede hash med den gemte hash (efter salt)
                for (int i = 0; i < KeySize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                        return false;
                }
                return true;
            }
        }
    }
}
