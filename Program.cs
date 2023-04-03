using LostWordDataDecrypter.DataTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LostWordDataDecrypter
{
    internal class Program
    {
        public const string ROOT_DIR = @".\";
        public const string INPUT_DIR = @"original_files";
        public const string OUTPUT_DIR = @"output_files";



        static void Main(string[] args)
        {
            bool doSeparateDecrypt = true;
            MasterMsgPack.Initialize();  // not sure if it's needed

            string[] dirs = Directory.GetDirectories(Path.Combine(ROOT_DIR, INPUT_DIR), "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in dirs)
            {
                Console.WriteLine("Processing: " + dir);


                /* Step 0: Creating output dir if exists
                 */
                string apkVersionName = new DirectoryInfo(dir).Name;
                string outputDir = Path.Combine(ROOT_DIR, Path.Combine(OUTPUT_DIR, apkVersionName));
                bool outputDirExists = Directory.Exists(outputDir);
                if (!outputDirExists)
                {
                    Console.WriteLine("creating output directory: " + outputDir);
                    Directory.CreateDirectory(outputDir);
                }

                /* Step 1: Find manifest file (first char is "{" and can fit into ManifestJSON)
                 */
                Console.WriteLine("Step 1: trying to find manifest file");
                string[] files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);
                Dictionary<string, string> tableToFileName = new Dictionary<string, string>();
                foreach (string file in files)
                {
                    using (StreamReader r = new StreamReader(file))
                    {
                        int header = r.Read();
                        if (header == Convert.ToInt32('{'))
                        {
                            Console.WriteLine("find a manifest file: " + file);
                            Console.Write("trying to convert to JSON ... ");
                            try
                            {
                                r.DiscardBufferedData();
                                using (StreamReader rr = new StreamReader(file))
                                {
                                    string json = rr.ReadToEnd();
                                    ManifestJSON manifestJSON = JsonConvert.DeserializeObject<ManifestJSON>(json);
                                    Console.WriteLine("successful");
                                    foreach (AssestInfo assestInfo in manifestJSON.AssetInfos)
                                    {
                                        if (assestInfo.IsMasterData())
                                        {
                                            string tableName = assestInfo.GetTableName();
                                            string tableFileName = assestInfo.Name;
                                            Console.WriteLine("table: " + tableName + ", file: " + tableFileName);
                                            tableToFileName[tableName] = tableFileName;
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("failed");
                            }
                            Console.WriteLine();
                        }
                    }
                }
                if (tableToFileName.Count == 0)
                {
                    foreach (string file in files)
                    {
                        Console.WriteLine(file);
                        using (StreamReader r = new StreamReader(file))
                        {
                            char[] header = new char[16];
                            r.Read(header, 0, 16);
                            string tableName = new string(header);
                            tableName = tableName.TrimEnd('\0');
                            if (Regex.IsMatch(tableName, @"^[a-zA-Z]+Table$"))
                            {
                                string tableFileName = Path.GetFileName(file);
                                Console.WriteLine("table: " + tableName + ", file: " + tableFileName);
                                tableToFileName[tableName] = tableFileName;
                            }
                        }
                    }
                }
                Console.WriteLine();

                /* Step 2: Decrypt each file
                 *   if we have tableToFileName, just look at each table
                 *   if not, then go through each file to see if the first 16 bytes (IV) are all alphabet chars
                 */
                if (doSeparateDecrypt)
                {
                    Console.WriteLine("Step 2: trying to decrypt files");
                    int succCount = 0;
                    foreach (string tableName in tableToFileName.Keys)
                    {
                        string fileName = tableToFileName[tableName];
                        Console.Write("Decrypting " + tableName + ": " + fileName + " ... ");
                        byte[] src = GeneralHelper.FileToBytes(Path.Combine(dir, fileName));
                        try
                        {
                            GeneralHelper.DecryptToFile(Path.Combine(outputDir, tableName + "(" + fileName + ")_decrypted"), src);
                            succCount++;
                            Console.WriteLine("successful");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("failed");
                        }

                    }
                    Console.WriteLine("Finished decrypting, successfult rate: {0}/{1}", succCount, tableToFileName.Count);
                    Console.WriteLine();
                }

                /* Step 3: Deserialize every data types
                 *   if the data type is not implemented or deserialize failed, the create an anonymous type
                 */
                Console.WriteLine("Step 3: trying to deserialize implemented data types");
                int succDataTypeCount = 0;
                int deserializeFailedDataTypeCount = 0;
                int unImplementedDataTypeCount = 0;
                foreach (string tableName in tableToFileName.Keys)
                {
                    string fileName = tableToFileName[tableName];
                    string outputFileName = Path.Combine(outputDir, tableName + ".csv");
                    Console.Write("Deserializing " + tableName + ": " + fileName.Substring(0, 10) + " ... ");
                    byte[] src = GeneralHelper.FileToBytes(Path.Combine(dir, fileName));

                    Type TableType = Type.GetType("LostWordDataDecrypter.DataTypes." + tableName);
                    if (TableType != null)
                    {
                        var unpackMethod = typeof(MasterMsgPack).GetMethod("Unpack").MakeGenericMethod(TableType);
                        try
                        {
                            // One example of the following code is like this:
                            // UnitTable st = MasterMsgPack.Unpack<UnitTable>(src);
                            // st.to_csv(Path.Combine(ROOT_DIR, Path.Combine(OUTPUT_DIR, table_name + ".csv")));
                            var st = unpackMethod.Invoke(null, new Object[] { src });
                            TableType.GetMethod("to_csv").Invoke(st, new Object[] { outputFileName });
                            Console.WriteLine("successful");
                            succDataTypeCount++;
                        }
                        //catch (Exception e)
                        catch (NotImplementedException e)
                        {
                            GeneralHelper.CreateFileForAnonymousType(src, outputFileName);
                            Console.WriteLine("type implemented but deserializing failed. Creating anonymous type");
                            deserializeFailedDataTypeCount++;
                        }
                    }
                    else
                    {
                        GeneralHelper.CreateFileForAnonymousType(src, outputFileName);
                        Console.WriteLine("type not implemented. Creating anonymous type");
                        unImplementedDataTypeCount++;
                    }
                }
                Console.WriteLine("Finished deserializing, successful rate: {0}/{1}/{2}", succDataTypeCount, deserializeFailedDataTypeCount, unImplementedDataTypeCount);
                Console.WriteLine();
            }

            Console.WriteLine("\npress any key to exit the process...");
            Console.ReadKey();
        }
    }
}
