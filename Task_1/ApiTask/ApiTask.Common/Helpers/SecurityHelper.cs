using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ApiTask.Common.Helpers
{

    public class SecurityHelper
    {
        public const string Salt = ".FvUf2hIMiDxoJJ!):ib6wKOB:gQ6y#4-0)=N=1CQ!FyM.jg@_";

        private const int _ITERATIONS = 100;
        private const int _OutPutLenght = 32;

        public static string PBKDF2Hash(string text, string salt = Salt)
        {
            return Convert.ToBase64String(GetPBKDF2(Encoding.UTF8.GetBytes(text), Encoding.UTF8.GetBytes(salt), _ITERATIONS, _OutPutLenght));
        }

        private static byte[] GetPBKDF2(byte[] value, byte[] salt, int iteration, int output_lenght)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(value, salt, iteration))
            {
                return pbkdf2.GetBytes(output_lenght);
            }
        }

        public static char[] GenerateSecurePassword()
        {
            char[] AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*".ToCharArray();
            char[] password = new char[8];

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[8];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < 8; i++)
                {
                    int index = randomBytes[i] % AllowedChars.Length;
                    password[i] = AllowedChars[index];
                }

                Array.Clear(randomBytes, 0, randomBytes.Length);
            }

            return password;
        }

        public static string GenerateAuthToken(int length = 32)
        {
            int byteLength = length / 2;
            Span<char> token = new char[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[byteLength];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < byteLength; i++)
                {
                    token[i * 2] = ToHexChar(randomBytes[i] >> 4);
                    token[i * 2 + 1] = ToHexChar(randomBytes[i] & 0x0F);
                }

                Array.Clear(randomBytes, 0, randomBytes.Length);
            }

            return new string(token);
        }

        public static string GeneratePrefixedToken(string prefix = "AUTH_")
        {
            string token = GenerateAuthToken();
            return prefix + token;
        }

        private static char ToHexChar(int value)
        {
            return (char)(value < 10 ? value + '0' : value - 10 + 'A');
        }
    }
}
