using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes
{
    [Serializable]
    public class SkillRecord : MasterRecord<int> // TypeDefIndex: 6982
    {
        public int id; // 0x8
        public string name; // 0xC
        public string description; // 0x10
        public int type; // 0x14
        public int exp_id; // 0x18
        public int level1_turn; // 0x1C
        public int level2_turn; // 0x20
        public int level3_turn; // 0x24
        public int level4_turn; // 0x28
        public int level5_turn; // 0x2C
        public int level6_turn; // 0x30
        public int level7_turn; // 0x34
        public int level8_turn; // 0x38
        public int level9_turn; // 0x3C
        public int level10_turn; // 0x40
        public int effect1_id; // 0x44
        public int effect1_level_type; // 0x48
        public int effect1_level_value; // 0x4C
        public int effect2_id; // 0x50
        public int effect2_level_type; // 0x54
        public int effect2_level_value; // 0x58
        public int effect3_id; // 0x5C
        public int effect3_level_type; // 0x60
        public int effect3_level_value; // 0x64
        public string icon_filename; // 0x68
        public string reincarnation_level_name; // 0x6C
    }

    [Serializable]
    public class SkillTable : MasterTable<int, SkillRecord>
    {

    }
    public class SkillMaster
    {
    }
}
