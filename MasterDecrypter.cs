using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LostWordDataDecrypter
{
    public class MasterDecrypter
    {
        private const int KEY_LENGTH = 16;
        private const int IV_LENGTH = 16;
        private const string SECRET_KEY = "bhVDLWfSBWureNR3";
        private const byte XOR = 100;

        private static byte[] GenerateEncryptionKey(string secret_key, byte xor)
        {
            char[] keyArr = secret_key.ToCharArray();
            for (int i = 0; i < secret_key.Length; i++)
            {
                char c = secret_key[i];
                byte b = Convert.ToByte(c);
                char newc = Convert.ToChar((uint)xor << ((byte)i & 1) ^ b & 0xff);
                keyArr[i] = newc;
            }

            string key = new string(keyArr);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] result = new byte[KEY_LENGTH];
            Array.Copy(keyBytes, 0, result, 0, KEY_LENGTH);
            return result;
        }

        public static byte[] Decrypt(byte[] src)
        {
            MemoryStream ifs = new MemoryStream(src);
            MemoryStream ofs = new MemoryStream();

            byte[] key_bytes = GenerateEncryptionKey(SECRET_KEY, XOR);
            byte[] iv_bytes = new byte[IV_LENGTH];
            ifs.Read(iv_bytes, 0, IV_LENGTH);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key_bytes;
                aesAlg.IV = iv_bytes;
                aesAlg.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                try
                {
                    using (CryptoStream csDecrypt = new CryptoStream(ifs, decryptor, CryptoStreamMode.Read))
                    {
                        int data;
                        while ((data = csDecrypt.ReadByte()) != -1)
                            ofs.WriteByte((byte)data);
                    }
                }
                catch (CryptographicException) { return new byte[0]; }
            }

            return ofs.ToArray();
        }
    }
}
