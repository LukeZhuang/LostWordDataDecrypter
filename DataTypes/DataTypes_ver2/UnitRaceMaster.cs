using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes.DataTypes_ver2
{
    [Serializable]
    public class UnitRaceRecord : MasterRecord<int>
    {
        public int id; // 0x8
        public int unit_id; // 0xC
        public int race_id; // 0x10
    }

    [Serializable]
    public class UnitRaceTable : MasterTable<int, UnitRaceRecord>
    {
    }
    public class UnitRaceMaster
    {
    }
}
