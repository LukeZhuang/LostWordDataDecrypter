using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes.DataTypes_ver2
{
    [Serializable]
    public class UnitRecord : MasterRecord<int>
    {
        public int id; // 0x8
        public string name; // 0xC
        public string name_kana; // 0x10
        public string alias_name; // 0x14
        public string short_name; // 0x18
        public int person_id; // 0x1C
        public int album_id; // 0x20
        public int role; // 0x24
        public int exp_id; // 0x28
        public string symbol_name; // 0x2C
        public string symbol_title; // 0x30
        public string symbol_description; // 0x34
        public int life_point; // 0x38
        public int yang_attack; // 0x3C
        public int yang_defense; // 0x40
        public int yin_attack; // 0x44
        public int yin_defense; // 0x48
        public int speed; // 0x4C
        public int shot1_id; // 0x50
        public int shot2_id; // 0x54
        public int spellcard1_id; // 0x58
        public int spellcard2_id; // 0x5C
        public int spellcard3_id; // 0x60
        public int spellcard4_id; // 0x64
        public int spellcard5_id; // 0x68
        public int skill1_id; // 0x6C
        public int skill2_id; // 0x70
        public int skill3_id; // 0x74
        public int resist_id; // 0x78
        public int characteristic_id; // 0x7C
        public int ability_id; // 0x80
        public int recycle_id; // 0x84
        public int default_costume_id; // 0x88
        public string drop_text; // 0x8C
        public int limitbreak_item_id; // 0x90
        public int spellcard_bgm_id; // 0x94
        public int is_show; // 0x98
    }

    [Serializable]
    public class UnitTable : MasterTable<int, UnitRecord>
    {
    }

    internal class UnitMaster
    {
    }
}
