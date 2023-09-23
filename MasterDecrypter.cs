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
        private const string SECRET_KEY = "jR3GTK6vR6gPz2MNA7hPiGEYbG6vBsMAS5jaretmHu4PS78zz3YcijFBM9yJha5HL7wn3DFYd4XaFNLB";

        private static byte[] GenerateBytes(string key)
        {
            Encoding utf8 = Encoding.UTF8;
            byte[] bytes = utf8.GetBytes(key);
            byte[] result = new byte[KEY_LENGTH];
            for (int i = 0; i < KEY_LENGTH; i++)
                result[i] = bytes[i];
            return result;
        }

        private static byte[] ShuffuleSrc(byte[] src, string encKey)
        {
            int srcLen = src.Length;
            byte[] modifiedSrc = new byte[srcLen];
            Array.Copy(src, 0, modifiedSrc, 0, srcLen);
            if (!string.IsNullOrEmpty(encKey))
            {
                int byteCount = (1 << (encKey.ToLower()[0] - 'a')) % (srcLen / 3);
                // which means move the last "byteCount" bytes to the beginning
                byte[] tmp = new byte[srcLen];
                Array.Copy(modifiedSrc, 0, tmp, 0, srcLen);
                Array.Copy(tmp, srcLen - byteCount, modifiedSrc, 0, byteCount);
                Array.Copy(tmp, 0, modifiedSrc, byteCount, srcLen - byteCount);
            }
            return modifiedSrc;
        }

        private static byte[] GenerateEncryptionKey(string tableName)
        {
            int index = tableName.Length;
            char[] keyArr = new char[KEY_LENGTH];
            for (int i = 0; i < KEY_LENGTH; i++)
            {
                byte nameByte = Convert.ToByte(tableName[i % tableName.Length]);
                byte keyByte = Convert.ToByte(SECRET_KEY[index % SECRET_KEY.Length]);
                keyArr[i] = Convert.ToChar(nameByte ^ keyByte);
                index += nameByte;
            }

            string key = new string(keyArr);
            Encoding utf8 = Encoding.UTF8;
            byte[] keyBytes = utf8.GetBytes(key);
            byte[] result = new byte[KEY_LENGTH];
            Array.Copy(keyBytes, 0, result, 0, KEY_LENGTH);
            return result;
        }

        public static byte[] Decrypt(byte[] src, string tableName, string enckey)
        {
            byte[] shuffuledSrc = ShuffuleSrc(src, enckey);

            MemoryStream ifs = new MemoryStream(shuffuledSrc);
            MemoryStream ofs = new MemoryStream();

            byte[] key_bytes = GenerateEncryptionKey(tableName);
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
