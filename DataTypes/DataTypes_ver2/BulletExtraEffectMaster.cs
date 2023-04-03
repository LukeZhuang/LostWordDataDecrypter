using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes.DataTypes_ver2
{
    [Serializable]
    public class BulletExtraEffectRecord : MasterRecord<int> // TypeDefIndex: 6653
    {
        public int id; // 0x8
        public string name; // 0xC
        public string description; // 0x10
        public int target; // 0x14
        public int turn; // 0x18
        public int type; // 0x1C
        public int subtype; // 0x20
        public int value; // 0x24
    }

    [Serializable]
    public class BulletExtraEffectTable : MasterTable<int, BulletExtraEffectRecord>
    {

    }
    public class BulletExtraEffectMaster
    {
    }
}
