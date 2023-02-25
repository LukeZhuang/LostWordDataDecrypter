using Microsoft.NET.StringTools;
using MsgPack;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static LostWordDataDecrypter.Program;

namespace LostWordDataDecrypter.DataTypes
{
    [Serializable]
    public abstract class MasterRecord<TKey>
    {
        public TKey PrimaryKey { get; }
    }


    public class MasterTable<TKey, TRec>
    {
        public Dictionary<TKey, TRec> m_table { get; set; }

        public MasterTable() {
            m_table = new Dictionary<TKey, TRec>();
        }

        public void to_csv(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                IList<string> field_names = new List<string>();
                FieldInfo[] fields = typeof(TRec).GetFields();
                foreach (FieldInfo field in fields)
                {
                    field_names.Add(field.Name);
                }
                string table_header = string.Join(",", field_names);
                writer.WriteLine(table_header);
                foreach (KeyValuePair<TKey, TRec> p in m_table)
                {
                    var record = p.Value;
                    IList<string> record_strs = new List<string>();
                    foreach (FieldInfo field in fields)
                    {
                        var value = field.GetValue(record);
                        string s = value.ToString();
                        s = s.Replace("\n", "\\n");
                        record_strs.Add(s);
                    }
                    string record_str = string.Join(",", record_strs);
                    writer.WriteLine(record_str);
                }
            }
        }

    }
}
