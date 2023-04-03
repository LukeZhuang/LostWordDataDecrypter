using Microsoft.NET.StringTools;
using MsgPack;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static LostWordDataDecrypter.Program;

namespace LostWordDataDecrypter.DataTypes.DataTypes_ver2
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

            /*
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
                        Type type = field.FieldType;
                        string s = value.ToString();
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
                        record_strs.Add(s);
                    }
                    string record_str = string.Join(",", record_strs);
                    writer.WriteLine(record_str);
                }
            }
            */
        }

    }
}
