using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes
{
    [Serializable]
    public class BulletRecord : MasterRecord<int>
    {
        public int id;
        public string name;
        public string description;
        public int element;
        public int type;
        public int category;
        public int power;
        public int hit;
        public int critical;
        public int bullet1_addon_id;
        public int bullet1_addon_value;
        public int bullet2_addon_id;
        public int bullet2_addon_value;
        public int bullet3_addon_id;
        public int bullet3_addon_value;
        public int bullet1_extraeffect_id;
        public int bullet1_extraeffect_success_rate;
        public int bullet2_extraeffect_id;
        public int bullet2_extraeffect_success_rate;
        public int bullet3_extraeffect_id;
        public int bullet3_extraeffect_success_rate;
    }
    
    [Serializable]
    public class BulletTable : MasterTable<int, BulletRecord>
    {
    }

    public class BulletMaster
    {
    }
}
