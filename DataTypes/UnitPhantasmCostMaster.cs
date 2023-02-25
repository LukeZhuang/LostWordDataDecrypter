using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes
{
    [Serializable]
    public class UnitPhantasmCostRecord : MasterRecord<long>
    {
        public int id; // 0x8
        public int rare; // 0xC
        public int level_limitbreak; // 0x10
        public int reincarnation_stage; // 0x14
        public int phantasm_cost; // 0x18
    }

    [Serializable]
    public class UnitPhantasmCostTable : MasterTable<long, UnitPhantasmCostRecord>
    {
    }
    public class UnitPhantasmCostMaster
    {
    }
}
