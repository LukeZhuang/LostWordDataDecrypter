using System;
using System.Collections.Generic;
using System.Reflection;

namespace LostWordDataDecrypter.DataTypes.DataTypes
{
    [Serializable]
    public abstract class MasterRecord<TKey>
    {
        public TKey PrimaryKey { get; }
    }


    public class MasterTable<TKey, TRec>
    {
        public Dictionary<TKey, TRec> m_table { get; set; }

        public MasterTable()
        {
            m_table = new Dictionary<TKey, TRec>();
        }

        public void ToCSV(string outputFilePath)
        {
            List<string> fieldNames = new List<string>();
            FieldInfo[] fields = typeof(TRec).GetFields();
            foreach (FieldInfo field in fields)
                fieldNames.Add(field.Name);

            List<List<object>> records = new List<List<object>>();
            foreach (KeyValuePair<TKey, TRec> p in m_table)
            {
                List<object> record = new List<object>();
                foreach (FieldInfo field in fields)
                {
                    var value = field.GetValue(p.Value);
                    record.Add(value);
                }
                records.Add(record);
            }
            GeneralHelper.TableToCSVFile(outputFilePath, fieldNames, records);
        }

        public void ToJSON(string outputFilePath)
        {
            List<string> jsonStrs = new List<string>();
            foreach (KeyValuePair<TKey, TRec> p in m_table)
            {
                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(p.Value);
                jsonStrs.Add(jsonStr);
            }
            GeneralHelper.TableToJSONFile(outputFilePath, jsonStrs);
        }

    }

    /* For version detecting */
    public class FakeMasterTable
    {

    }
}
