using LostWordDataDecrypter.DataTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace LostWordDataDecrypter
{
    internal class Program
    {
        public const string ROOT_DIR = @".\";

        static void Main(string[] args)
        {
            string input;

            Console.WriteLine("Please enter the version name as the workspace directory: (any valid format, for example \"ch_master_v6008\")");
            input = Console.ReadLine();
            string sourceDir = Path.Combine(ROOT_DIR, input);
            string destinationDir = Path.Combine(sourceDir, "output");
            if (!Directory.Exists(sourceDir))
            {
                Console.WriteLine("creating output directory: " + sourceDir);
                Directory.CreateDirectory(sourceDir);
            }
            else
            {
                Console.WriteLine("Directory already exists!");
                goto End;
            }

            Console.WriteLine("Please input the master url: (starts with \"https\", ends with \"/\")");
            string download_url_prefix = Console.ReadLine();
            if (!download_url_prefix.StartsWith("https") || !download_url_prefix.EndsWith("/"))
            {
                Console.WriteLine("Please check url format!");
                goto End;
            }

            Console.WriteLine("trying download manifest...");
            string manifestFilePath = Path.Combine(sourceDir, "manifest.json");
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(download_url_prefix + "manifest.json", manifestFilePath);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Fail to download " + download_url_prefix + "manifest.json" + ", file does not exist!");
                goto End;
            }

            Dictionary<string, string> tableToFileName;
            try
            {
                tableToFileName = GeneralHelper.GetTableInfoFromManifest(manifestFilePath);
            }
            catch (Exception)
            {
                Console.WriteLine("Manifest file format is not valid, please check its content!");
                goto End;
            }

            if (tableToFileName.Count == 0)
            {
                Console.WriteLine("Cannot find master table in manifest file, please check its content!");
                goto End;
            }
            Console.WriteLine("Successfully get table info list!");
            
            foreach (string tableName in tableToFileName.Keys)
            {
                string fileName = tableToFileName[tableName];
                Console.WriteLine(string.Format("downloading table {0} file {1}", tableName, fileName));
                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(download_url_prefix + fileName, Path.Combine(sourceDir, fileName));
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Fail to download table " + tableName + ", exit!");
                    goto End;
                }
            }

            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            bool writeDecryptFile = false;
            bool writeCSVFile, writeJSONFile;

            Console.WriteLine("Choose output formats:");
            Console.WriteLine("1: only csv");
            Console.WriteLine("2: only json");
            Console.WriteLine("3: both csv and json");

            input = Console.ReadLine();
            if (input.Equals("1"))
            {
                writeCSVFile = true;
                writeJSONFile = false;
            }
            else if (input.Equals("2"))
            {
                writeCSVFile = false;
                writeJSONFile = true;
            }
            else if (input.Equals("3"))
            {
                writeCSVFile = writeJSONFile = true;
            }
            else
            {
                Console.WriteLine("Invalid input!");
                goto End;
            }

            int totalFile = 0, success = 0, decryptFail = 0, deserializeFail = 0, anonymousDeserializeFail = 0;
            foreach (string tableName in tableToFileName.Keys)
            {
                string fileName = tableToFileName[tableName];
                GeneralHelper.DecryptStatus res = GeneralHelper.DecryptFile(tableName, sourceDir, destinationDir, fileName, writeDecryptFile, writeCSVFile, writeJSONFile);
                if (res == GeneralHelper.DecryptStatus.Success)
                {
                    Console.WriteLine(string.Format("Decrypting and Deserializing {0}({1}) successful.", tableName, fileName));
                    success++;
                }
                else if (res == GeneralHelper.DecryptStatus.DecryptionFail)
                {
                    Console.WriteLine(string.Format("Decrypting {0}({1}) failed.", tableName, fileName));
                    decryptFail++;
                }
                else if (res == GeneralHelper.DecryptStatus.DeserializeFail)
                {
                    Console.WriteLine(string.Format("Decrypting {0}({1}) successfully but deserializing failed, creating anonymous json.", tableName, fileName));
                    deserializeFail++;
                }
                else if (res == GeneralHelper.DecryptStatus.AnonymousDeserializeFail)
                {
                    Console.WriteLine(string.Format("Decrypting {0}({1}) successfully but deserializing failed, and not able to create anonymous json.", tableName, fileName));
                    anonymousDeserializeFail++;
                }
                else
                {
                    throw new InvalidDataException("unhandled decrypt status type");
                }
                totalFile++;
            }

            Console.WriteLine("\nJob finished!");
            Console.WriteLine("Number of total tables: " + totalFile);
            Console.WriteLine("Number of successful tables: " + success);
            Console.WriteLine("Number of decrypting failed tables: " + decryptFail);
            Console.WriteLine("Number of deserializing failed tables (still created anonymous json): " + deserializeFail);
            Console.WriteLine("Number of deserializing failed tables (cannot create anonymous json): " + anonymousDeserializeFail);

            // clean files
            foreach (string file in Directory.GetFiles(sourceDir))
                File.Delete(file);

        End:
            Console.WriteLine("\npress any key to exit the process...");
            Console.ReadKey();
        }
    }
}
