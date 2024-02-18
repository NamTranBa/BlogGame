using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAL
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16; // Độ dài của salt
        private const int KeySize = 32; // Độ dài của hash
        private const int Iterations = 10000; // Số lần lặp, càng cao càng an toàn nhưng sẽ chậm hơn
        public static byte[] HashPassword(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] salt = algorithm.Salt;
                byte[] hash = algorithm.GetBytes(KeySize);

                byte[] hashBytes = new byte[SaltSize + KeySize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

                return hashBytes;
            }
        }
        public static bool VerifyPassword(byte[] storedHash, string password)
        {
            byte[] salt = new byte[SaltSize];
            Array.Copy(storedHash, 0, salt, 0, SaltSize);
            using (var algorithm = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hashToVerify = algorithm.GetBytes(KeySize);
                for (int i = 0; i < KeySize; i++)
                {
                    if (hashToVerify[i] != storedHash[i + SaltSize])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
