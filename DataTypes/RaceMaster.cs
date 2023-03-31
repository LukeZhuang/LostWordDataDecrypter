using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes
{
    [Serializable]
    public class RaceRecord : MasterRecord<int>
    {
        public int id;
        public string name;
        public string description;
        public string alias_name;
    }

    [Serializable]
    public class RaceTable : MasterTable<int, RaceRecord>
    {

    }
    public class RaceMaster
    {
    }
}
