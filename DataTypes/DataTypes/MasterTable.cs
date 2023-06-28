using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public void ToCSV(string fullPath)
        {
            List<string> fieldNames = new List<string>();
            FieldInfo[] fields = typeof(TRec).GetFields();
            foreach (FieldInfo field in fields)
            {
                fieldNames.Add(field.Name);
            }

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
            GeneralHelper.TableToCSV(fullPath, fieldNames, records);
        }

        public void ToJSON(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                foreach (KeyValuePair<TKey, TRec> p in m_table)
                {
                    string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(p.Value);
                    writer.WriteLine(jsonStr);
                }
            }
        }

    }

    /* For version detecting */
    public class FakeMasterTable
    {

    }
}
