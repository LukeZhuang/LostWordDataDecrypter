using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes
{
    [Serializable]
    public class PersonRelationRecord : MasterRecord<long> // TypeDefIndex: 6880
    {
        // Fields
        public int id; // 0x8
        public int person_id; // 0xC
        public int target_person_id; // 0x10
        public string description; // 0x14
    }
    
    [Serializable]
    public class PersonRelationTable : MasterTable<long, PersonRelationRecord>
    {

    }
    public class PersonRelationMaster
    {
    }
}
