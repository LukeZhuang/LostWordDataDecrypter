using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes.DataTypes_ver2
{
    [Serializable]
    public class SpellcardRecord : MasterRecord<int> 
    {
        public int id; 
        public string name; 
        public string description; 
        public string specification; 
        public int type; 
        public int magazine0_bullet_id; 
        public int magazine0_bullet_range; 
        public int magazine0_bullet_value; 
        public int magazine0_bullet_power_rate; 
        public int magazine1_boost_count; 
        public int magazine1_bullet_id; 
        public int magazine1_bullet_range; 
        public int magazine1_bullet_value; 
        public int magazine1_bullet_power_rate; 
        public int magazine2_boost_count; 
        public int magazine2_bullet_id; 
        public int magazine2_bullet_range; 
        public int magazine2_bullet_value; 
        public int magazine2_bullet_power_rate; 
        public int magazine3_boost_count; 
        public int magazine3_bullet_id; 
        public int magazine3_bullet_range; 
        public int magazine3_bullet_value; 
        public int magazine3_bullet_power_rate; 
        public int magazine4_boost_count; 
        public int magazine4_bullet_id; 
        public int magazine4_bullet_range; 
        public int magazine4_bullet_value; 
        public int magazine4_bullet_power_rate; 
        public int magazine5_boost_count; 
        public int magazine5_bullet_id; 
        public int magazine5_bullet_range; 
        public int magazine5_bullet_value; 
        public int magazine5_bullet_power_rate; 
        public int phantasm_power_up_rate; 
        public int shot_level0_power_rate; 
        public int shot_level1_power_rate; 
        public int shot_level2_power_rate; 
        public int shot_level3_power_rate; 
        public int shot_level4_power_rate; 
        public int shot_level5_power_rate; 
        public int spellcard_skill1_effect_id; 
        public int spellcard_skill1_level_type; 
        public int spellcard_skill1_level_value; 
        public int spellcard_skill1_timing; 
        public int spellcard_skill2_effect_id; 
        public int spellcard_skill2_level_type; 
        public int spellcard_skill2_level_value; 
        public int spellcard_skill2_timing; 
        public int spellcard_skill3_effect_id; 
        public int spellcard_skill3_level_type; 
        public int spellcard_skill3_level_value; 
        public int spellcard_skill3_timing; 
        public int spellcard_skill4_effect_id; 
        public int spellcard_skill4_level_type; 
        public int spellcard_skill4_level_value; 
        public int spellcard_skill4_timing;
        public int spellcard_skill5_effect_id;
        public int spellcard_skill5_level_type;
        public int spellcard_skill5_level_value;
        public int spellcard_skill5_timing;
    }
    
    [Serializable]
    public class SpellcardTable : MasterTable<int, SpellcardRecord>
    {
    }
    public class SpellcardMaster
    {
    }
}
