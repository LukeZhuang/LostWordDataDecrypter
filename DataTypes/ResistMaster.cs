using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes
{
    [Serializable]
    public class ResistRecord : MasterRecord<int> // TypeDefIndex: 6943
    {
        public int id; // 0x8
        public string name; // 0xC
        public string description; // 0x10
        public int element1_resistance; // 0x14
        public int element2_resistance; // 0x18
        public int element3_resistance; // 0x1C
        public int element4_resistance; // 0x20
        public int element5_resistance; // 0x24
        public int element6_resistance; // 0x28
        public int element7_resistance; // 0x2C
        public int element8_resistance; // 0x30 
    }

    [Serializable]
    public class ResistTable : MasterTable<int, ResistRecord>
    {

    }
    public class ResistMaster
    {
    }
}
