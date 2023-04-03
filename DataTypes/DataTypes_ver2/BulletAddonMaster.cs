using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes.DataTypes_ver2
{
    [Serializable]
    public class BulletAddonRecord : MasterRecord<int> // TypeDefIndex: 6647
    {
        public int id; // 0x8
        public string name; // 0xC
        public string description; // 0x10
    }
    
    [Serializable]
    public class BulletAddonTable : MasterTable<int, BulletAddonRecord>
    {

    }
    public class BulletAddonMaster
    {
    }
}
