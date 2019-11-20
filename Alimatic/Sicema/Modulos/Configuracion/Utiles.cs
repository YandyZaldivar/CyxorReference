using System;
using System.Text;
using System.Security.Cryptography;

namespace Sicema
{
    public static class Utiles
    {
        public static class Crypto
        {
            public static string EncryptText(SymmetricAlgorithm symmetricAlgorithm, string text)
            {
                using (var encryptor = symmetricAlgorithm.CreateEncryptor())
                {
                    var bytes = Encoding.UTF8.GetBytes(text);
                    var result = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                    return Convert.ToBase64String(result);
                }
            }

            public static string DecryptText(SymmetricAlgorithm symmetricAlgorithm, string text)
            {
                using (var decryptor = symmetricAlgorithm.CreateDecryptor())
                {
                    var bytes = Convert.FromBase64String(text);
                    var result = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                    return Encoding.UTF8.GetString(result);
                }
            }

            public static SymmetricAlgorithm CreateSymmetricAlgorithm(string password)
            {
                var symmetricAlgorithm = Aes.Create();

                symmetricAlgorithm.KeySize = 256;
                symmetricAlgorithm.BlockSize = 128;
                symmetricAlgorithm.Mode = CipherMode.CBC;
                symmetricAlgorithm.Padding = PaddingMode.PKCS7;

                var salt = new byte[] { 72, 112, 8, 82, 200, 11, 178, 29, 92, 90, 3, 7, 13, 14, 211, 16 };

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations: 7))
                {
                    symmetricAlgorithm.Key = pbkdf2.GetBytes(symmetricAlgorithm.KeySize / 8);
                    symmetricAlgorithm.IV = pbkdf2.GetBytes(symmetricAlgorithm.BlockSize / 8);
                }

                return symmetricAlgorithm;
            }
        }
    }
}
