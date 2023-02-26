using LostWordDataDecrypter.DataTypes;
using MsgPack.Serialization;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter
{
    internal class Program
    {
        public const string ROOT_DIR = @".\";
        public const string INPUT_DIR = @"original_files";
        public const string OUTPUT_DIR = @"output_files";

        internal class FileEncryptor
        {
            public const int KEY_LENGTH = 16;
            public const int IV_LENGTH = 16;

            public static byte[] GenerateBytes(string password, int length)
            {
                Encoding utf8 = Encoding.UTF8;
                byte[] bytes = utf8.GetBytes(password);
                byte[] result = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = bytes[i];
                }
                return result;
            }

            public static void Decrypt(Stream ifs, Stream ofs, string key)
            {
                byte[] key_bytes = GenerateBytes(key, KEY_LENGTH);
                byte[] iv_bytes = new byte[IV_LENGTH];
                ifs.Read(iv_bytes, 0, IV_LENGTH);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = key_bytes;
                    aesAlg.IV = iv_bytes;
                    aesAlg.Mode = CipherMode.CBC;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(ifs, decryptor, CryptoStreamMode.Read))
                    {
                        int data;
                        while ((data = csDecrypt.ReadByte()) != -1)
                            ofs.WriteByte((byte)data);
                    }
                }
            }
        }
        internal static class MasterMsgPack
        {
            public const string SECRET_KEY = "bhVDLWfSBWureNR3";
            public const byte XOR = 100;
            public static SerializationContext m_context;

            public static void Initialize()
            {
                MsgPack.Serialization.MessagePackSerializer.PrepareType<int>();
                MsgPack.Serialization.MessagePackSerializer.PrepareType<long>();
                MsgPack.Serialization.MessagePackSerializer.PrepareType<float>();
                m_context = MsgPack.Serialization.SerializationContext.Default;
                m_context.SerializationMethod = 0;
            }

            public static string GenerateEncryptionKey(string secret_key, byte xor)
            {
                char[] result = secret_key.ToCharArray();
                for (int i = 0; i < secret_key.Length; i++)
                {
                    char c = secret_key[i];
                    byte b = System.Convert.ToByte(c);
                    char newc = System.Convert.ToChar((uint)xor << ((byte)i & 1) ^ b & 0xff);
                    result[i] = newc;
                }
                return new string(result);
            }

            public static byte[] Decrypt(byte[] src)
            {
                MemoryStream ifs = new MemoryStream(src);
                MemoryStream ofs = new MemoryStream();
                string key = MasterMsgPack.GenerateEncryptionKey(SECRET_KEY, XOR);
                FileEncryptor.Decrypt(ifs, ofs, key);
                byte[] res = ofs.ToArray();
                return res;
            }

            public static T Unpack<T>(byte[] src)
            {
                byte[] output_buff = MasterMsgPack.Decrypt(src);
                byte[] newbuff = new byte[output_buff.Length + 1];
                newbuff[0] = (byte)(0x91);  // magic number
                for (int i = 0; i < output_buff.Length; i++)
                {
                    newbuff[i + 1] = output_buff[i];
                }

                return m_context.GetSerializer<T>().UnpackSingleObject(newbuff);
            }
        }

        internal class TestDecoder
        {
            public static byte[] FileToBytes(string input_filename)
            {
                byte[] input_buff = null;
                FileStream ifs = new FileStream(input_filename, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(ifs);
                long numBytes = new FileInfo(input_filename).Length;
                input_buff = br.ReadBytes((int)numBytes);
                return input_buff;
            }
            public static void DecryptToFile(string output_filename, byte[] src)
            {
                byte[] output_buff = MasterMsgPack.Decrypt(src);
                FileStream ofs = File.Create(output_filename);
                ofs.Write(output_buff, 0, output_buff.Length);
            }
        }


        static void Main(string[] args)
        {
            MasterMsgPack.Initialize();  // not sure if it's needed

            string[] dirs = Directory.GetDirectories(Path.Combine(ROOT_DIR, INPUT_DIR), "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in dirs)
            {
                string apkVersionName = new DirectoryInfo(dir).Name;

                string masterDir = Path.Combine(dir, "master");
                Console.WriteLine("Processing: " + masterDir);

                string outputDir = Path.Combine(ROOT_DIR, Path.Combine(OUTPUT_DIR, apkVersionName));
                bool outputDirExists = Directory.Exists(outputDir);
                if (!outputDirExists)
                {
                    Console.WriteLine("creating output directory: " + outputDir);
                    Directory.CreateDirectory(outputDir);
                }


                List<(string, string)> validTableNames = new List<(string, string)>();

                using (StreamReader r = new StreamReader(Path.Combine(masterDir, "Manifest.json")))
                {
                    string json = r.ReadToEnd();
                    ManifestJSON manifestJSON = JsonConvert.DeserializeObject<ManifestJSON>(json);
                    foreach (AssestInfo assestInfo in manifestJSON.AssetInfos)
                    {
                        if (assestInfo.IsMasterData())
                        {
                            string tableName = assestInfo.GetTableName();
                            Type MasterDataType = Type.GetType("LostWordDataDecrypter.DataTypes." + tableName);
                            if (MasterDataType != null)
                            {
                                validTableNames.Add((tableName, assestInfo.Name));
                            }
                        }
                    }
                }

                foreach ((string tableName, string fileName) in validTableNames)
                {
                    Console.WriteLine("Decoding " + tableName + ": " + fileName);
                    Type TableType = Type.GetType("LostWordDataDecrypter.DataTypes." + tableName, true);
                    byte[] src = TestDecoder.FileToBytes(Path.Combine(masterDir, fileName));
                    //TestDecoder.DecryptToFile(Path.Combine(outputDir, tableName + "_decrypted"), src);

                    // One example of the following code is like this:
                    // UnitTable st = MasterMsgPack.Unpack<UnitTable>(src);
                    // st.to_csv(Path.Combine(ROOT_DIR, Path.Combine(OUTPUT_DIR, table_name + ".csv")));
                    var st = typeof(MasterMsgPack).GetMethod("Unpack").MakeGenericMethod(TableType).Invoke(null, new Object[] { src });
                    TableType.GetMethod("to_csv").Invoke(st, new Object[] { Path.Combine(outputDir, tableName + ".csv") });
                }
                Console.WriteLine();
            }

            Console.WriteLine("\npress any key to exit the process...");
            Console.ReadKey();
        }
    }
}
