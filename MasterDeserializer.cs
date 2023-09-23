using MsgPack.Serialization;
using System;
using System.IO;
using System.Reflection;

namespace LostWordDataDecrypter
{
    public class MasterDeserializer
    {
        public static T Unpack<T>(byte[] decryptOutput)
        {
            byte[] buff = new byte[decryptOutput.Length + 1];
            buff[0] = 0x91;  // unpack needs a list wrapper
            for (int i = 0; i < decryptOutput.Length; i++)
                buff[i + 1] = decryptOutput[i];
            return SerializationContext.Default.GetSerializer<T>().UnpackSingleObject(buff);
        }

        public static bool UnpackToFile(byte[] decryptOutput, string tableName, string outputDirectory, bool writeCSVFile, bool writeJSONFile)
        {
            Type tableType = Type.GetType("LostWordDataDecrypter.DataTypes.DataTypes." + tableName);
            if (tableType == null)
                return false;
            var unpackMethod = typeof(MasterDeserializer).GetMethod("Unpack").MakeGenericMethod(tableType);
            try
            {
                var st = unpackMethod.Invoke(null, new object[] { decryptOutput });
                if (writeCSVFile)
                    tableType.GetMethod("ToCSV").Invoke(st, new object[] { Path.Combine(outputDirectory, tableName + ".csv") });
                if (writeJSONFile)
                    tableType.GetMethod("ToJSON").Invoke(st, new object[] { Path.Combine(outputDirectory, tableName + ".json") });
                return true;
            }
            catch (TargetInvocationException)
            {
                return false;
            }
        }
    }
}
