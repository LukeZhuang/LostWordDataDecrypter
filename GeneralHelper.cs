using LostWordDataDecrypter.DataTypes;
using MsgPack;
using MsgPack.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LostWordDataDecrypter
{
    public class GeneralHelper
    {
        public enum DecryptStatus {
            Success = 0,
            DecryptionFail = 1,
            DeserializeFail = 2,
            AnonymousDeserializeFail = 3,
        }
        private static byte[] TryDecrypt(string inputFilePath, string tableName)
        {
            byte[] originalBytes = File.ReadAllBytes(inputFilePath);
            for (int key = -1; key <= 26; key++)
            {
                string encKey = (key == -1) ? "" : new string(Convert.ToChar('a' + key), 1);
                byte[] decryptOutput = MasterDecrypter.Decrypt(originalBytes, tableName, encKey);
                if (decryptOutput.Length == 0)
                    continue;
                return decryptOutput;
            }
            return new byte[0];
        }

        public static DecryptStatus DecryptFile(string tableName, string inputDirectory, string outputDirectory, string originalFileName, 
                                                bool writeDecryptFile, bool writeCSVFile, bool writeJSONFile)
        {
            byte[] decryptOutput = TryDecrypt(Path.Combine(inputDirectory, originalFileName), tableName);
            if (decryptOutput.Length == 0)
                return DecryptStatus.DecryptionFail;

            if (writeDecryptFile)
                File.WriteAllBytes(Path.Combine(outputDirectory, tableName + "(" + originalFileName + ")_decrypted"), decryptOutput);

            bool unpackSucc = MasterDeserializer.UnpackToFile(decryptOutput, tableName, outputDirectory, writeCSVFile, writeJSONFile);
            if (!unpackSucc)
            {
                try
                {
                    CreateJSONFileForAnonymousType(decryptOutput, Path.Combine(outputDirectory, tableName + ".anonymous.json"));
                }
                catch (Exception)
                {
                    return DecryptStatus.AnonymousDeserializeFail;
                }
                return DecryptStatus.DeserializeFail;
            }
            return DecryptStatus.Success;
        }

        public static void TableToCSVFile(string csvOutputPath, List<string> fieldNames, List<List<object>> records)
        {
            using (StreamWriter writer = new StreamWriter(csvOutputPath))
            {
                writer.WriteLine(string.Join(",", fieldNames));
                foreach (var record in records)
                {
                    IList<string> recordStrs = new List<string>();
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
                        recordStrs.Add(s);
                    }
                    string recordStr = string.Join(",", recordStrs);
                    writer.WriteLine(recordStr);
                }
            }
        }

        public static void TableToJSONFile(string jsonOutputPath, List<string> jsonStrs)
        {
            using (StreamWriter writer = new StreamWriter(jsonOutputPath))
            {
                foreach (string jsonStr in jsonStrs)
                    writer.WriteLine(jsonStr);
            }
        }

        private static void CreateJSONFileForAnonymousType(byte[] decryptOutput, string anonymousOutputPath)
        {
            using (StreamWriter writer = new StreamWriter(anonymousOutputPath))
            {
                Stream stream = new MemoryStream(decryptOutput);
                MessagePackObject data = MessagePackSerializer.UnpackMessagePackObject(stream);
                string jsonStr = data.ToString();
                JObject json = JObject.Parse(jsonStr);
                writer.Write(json);
            }
        }

        public static string AutoDetectManifestFile(string sourceDir)
        {
            if (Directory.Exists(Path.Combine(sourceDir, "manifest.json")))
                return Path.Combine(sourceDir, "manifest.json");

            string[] files = Directory.GetFiles(sourceDir, "*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                using (StreamReader r = new StreamReader(file))
                {
                    int header = r.Read();
                    if (header == Convert.ToInt32('{'))
                    {
                        try
                        {
                            string json;
                            using (StreamReader ifs = new StreamReader(file))
                            {
                                json = ifs.ReadToEnd();
                                ManifestJSON manifestJSON = JsonConvert.DeserializeObject<ManifestJSON>(json);
                            }
                            if (json.Contains("Master.bytes"))
                                return file;
                        } catch (Exception) { }
                    }
                }
            }
            return "";
        }

        public static Dictionary<string, string> GetTableInfoFromManifest(string manifestFilePath)
        {
            Dictionary<string, string> tableToFileName = new Dictionary<string, string>();
            using (StreamReader ifs = new StreamReader(manifestFilePath))
            {
                string json = ifs.ReadToEnd();
                ManifestJSON manifestJSON = JsonConvert.DeserializeObject<ManifestJSON>(json);
                foreach (AssestInfo assestInfo in manifestJSON.AssetInfos)
                {
                    if (assestInfo.IsMasterData())
                    {
                        string tableName = assestInfo.GetTableName();
                        string tableFileName = assestInfo.Name;
                        tableToFileName[tableName] = tableFileName;
                    }
                }
            }
            return tableToFileName;
        }

    }
}
