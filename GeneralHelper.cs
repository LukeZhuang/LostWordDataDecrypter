using MsgPack;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LostWordDataDecrypter
{
    public class FileEncryptor
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

    public static class MasterMsgPack
    {
        public const string SECRET_KEY = "jR3GTK6vR6gPz2MNA7hPiGEYbG6vBsMAS5jaretmHu4PS78zz3YcijFBM9yJha5HL7wn3DFYd4XaFNLB";
        public static SerializationContext m_context;

        public static void Initialize()
        {
            MsgPack.Serialization.MessagePackSerializer.PrepareType<int>();
            MsgPack.Serialization.MessagePackSerializer.PrepareType<long>();
            MsgPack.Serialization.MessagePackSerializer.PrepareType<float>();
            m_context = MsgPack.Serialization.SerializationContext.Default;
            m_context.SerializationMethod = 0;
        }

        public static int GetByteCount(string enckey)
        {
            char c = enckey[0];
            byte a = Convert.ToByte(c);
            byte b = Convert.ToByte(0x61);
            if ((a & 0xff) < (b & 0xff))
                b = Convert.ToByte(0x41);
            return 1 << ((a & 0xff) - (b & 0x1f));
        }

        public static string GenerateEncryptionKey(string name, int length, ref int index)
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                byte nameByte = Convert.ToByte(name[i % name.Length]);
                byte keyByte = Convert.ToByte(SECRET_KEY[index % SECRET_KEY.Length]);
                result[i] = Convert.ToChar(nameByte ^ keyByte);
                index += nameByte;
            }
            return new string(result);
        }

        public static byte[] Decrypt(byte[] src, string name, string enckey)
        {
            int length = src.Length;
            byte[] modifiedSrc = new byte[length];
            Array.Copy(src, 0, modifiedSrc, 0, length);
            if (!String.IsNullOrEmpty(enckey))
            {
                int byteCount = GetByteCount(enckey) % (src.Length / 3);
                // which means move the last "byteCount" bytes to the beginning
                byte[] tmp = new byte[length];
                Array.Copy(modifiedSrc, 0, tmp, 0, length);
                Array.Copy(tmp, length - byteCount, modifiedSrc, 0, byteCount);
                Array.Copy(tmp, 0, modifiedSrc, byteCount, length - byteCount);
            }
            MemoryStream ifs = new MemoryStream(modifiedSrc);
            MemoryStream ofs = new MemoryStream();
            int index = name.Length;
            string key = GenerateEncryptionKey(name, 0x10, ref index);
            FileEncryptor.Decrypt(ifs, ofs, key);
            return ofs.ToArray();
        }

        public static T Unpack<T>(byte[] src, string name, string enckey)
        {
            byte[] output_buff = MasterMsgPack.Decrypt(src, name, enckey);
            byte[] newbuff = new byte[output_buff.Length + 1];
            newbuff[0] = (byte)(0x91);  // unpack needs a list wrapper
            for (int i = 0; i < output_buff.Length; i++)
            {
                newbuff[i + 1] = output_buff[i];
            }
            return m_context.GetSerializer<T>().UnpackSingleObject(newbuff);
        }

    }

    public class GeneralHelper
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
        public static void DecryptToFile(string output_filename, byte[] src, string name, string enckey)
        {
            byte[] output_buff = MasterMsgPack.Decrypt(src, name, enckey);
            FileStream ofs = File.Create(output_filename);
            ofs.Write(output_buff, 0, output_buff.Length);
        }
        public static void HandleFields(MessagePackObject field, List<string> fieldNames, List<object> record,
                                        ref int intTypeId, ref int strTypeId, ref int floatTypeId, bool createHeader)
        {
            if (field.IsTypeOf<Int32>().GetValueOrDefault())
            {
                int v = field.AsInt32();
                record.Add(v);
                if (createHeader)
                {
                    fieldNames.Add("AnonymousInt" + intTypeId);
                    intTypeId++;
                }
            }
            else if (field.IsTypeOf<string>().GetValueOrDefault())
            {
                string s = field.AsString();
                record.Add(s);
                if (createHeader)
                {
                    fieldNames.Add("AnonymousStr" + strTypeId);
                    strTypeId++;
                }
            }
            else if (field.IsTypeOf<Single>().GetValueOrDefault())
            {
                float f = field.AsSingle();
                record.Add(f);
                if (createHeader)
                {
                    fieldNames.Add("AnonymousFloat" + floatTypeId);
                    floatTypeId++;
                }
            }
            else if (field.IsNil)
            {
                record.Add("");
                if (createHeader)
                {
                    fieldNames.Add("AnonymousStr" + strTypeId);
                    strTypeId++;
                }
            }
            else if (field.IsList)
            {
                var fieldList = field.AsList();
                List<string> strs = new List<string>();
                foreach (var subfield in fieldList)
                {
                    if (!subfield.IsTypeOf<Int32>().GetValueOrDefault())
                        throw new NotImplementedException();
                    strs.Add(subfield.AsInt32().ToString());
                }
                string concatStr = string.Join("|", strs);
                record.Add(concatStr);
                if (createHeader)
                {
                    fieldNames.Add("AnonymousStr" + strTypeId);
                    strTypeId++;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public static void CreateFileForAnonymousType(byte[] src, string fullPath, string name, string enckey)
        {
            byte[] output_buff = MasterMsgPack.Decrypt(src, name, enckey);
            Stream stream = new MemoryStream(output_buff);

            List<string> fieldNames = new List<string>();
            List<List<object>> records = new List<List<object>>();
            int intTypeId = 1;
            int strTypeId = 1;
            int floatTypeId = 1;
            bool createHeader = true;

            using (var unpacker = Unpacker.Create(stream))
            {
                for (var data = unpacker.ReadItem(); data != null; data = unpacker.ReadItem())
                {
                    MessagePackObjectDictionary dict = data.Value.AsDictionary();
                    foreach (var key in dict.Keys)
                    {
                        var anonymousList = dict[key].AsList();
                        List<object> record = new List<object>();
                        foreach (var field in anonymousList)
                        {
                            HandleFields(field, fieldNames, record, ref intTypeId, ref strTypeId, ref floatTypeId, createHeader);
                        }
                        createHeader = false;
                        records.Add(record);
                    }
                }
            }
            TableToCSV(fullPath, fieldNames, records);
        }
        public static void TableToCSV(string fullPath, List<string> fieldNames, List<List<object>> records)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine(string.Join(",", fieldNames));
                foreach (var record in records)
                {
                    IList<string> record_strs = new List<string>();
                    foreach (var field in record)
                    {
                        string s = "";
                        if (field != null)
                        {
                            Type type = field.GetType();
                            s = field.ToString();
                            if (type == typeof(string))
                            {
                                s = s.Replace("\r\n", "<ret>");
                                s = s.Replace("\n", "<ret>");
                                s = s.Replace(",", "|");
                                string emptyPattern = @"^\s*$";
                                Match m = Regex.Match(s, emptyPattern, RegexOptions.IgnoreCase);
                                if (m.Success)
                                {
                                    s = "(empty)";
                                }
                            }
                            else if (type != typeof(int) && type != typeof(float))
                            {
                                /* currently only support int, float and string */
                                throw new NotImplementedException();
                            }
                        } else
                        {
                            s = "(empty)";
                        }
                        record_strs.Add(s);
                    }
                    string record_str = string.Join(",", record_strs);
                    writer.WriteLine(record_str);
                }
            }
        }

    }
}
