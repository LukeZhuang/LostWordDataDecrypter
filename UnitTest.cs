using LostWordDataDecrypter.DataTypes;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LostWordDataDecrypter.Program;

namespace LostWordDataDecrypter
{
    internal static class UnitTest
    {
        public static void do_unit_test()
        {
            byte[] data = null;

            /*
            var a1 = new UnitRaceRecord
            {
                id = 1,
                unit_id = 1001,
                race_id = 1,
            };
            var a2 = new UnitRaceRecord
            {
                id = 2,
                unit_id = 1001,
                race_id = 2,
            };
            var a3 = new UnitRaceRecord
            {
                id = 3,
                unit_id = 1001,
                race_id = 52,
            };
            UnitRaceTable t = new UnitRaceTable();
            t.m_table.Add(1, a1);
            t.m_table.Add(2, a2);
            t.m_table.Add(3, a3);

            //MessagePackSerializer.PrepareDictionaryType<int, UnitRaceTable>();
            data = MasterMsgPack.m_context.GetSerializer<UnitRaceTable>().PackSingleObject(t);
            */

            /*
            var a1 = new UnitPhantasmCostRecord
            {
                id = 1,
                rare = 1,
                level_limitbreak = 0,
                phantasm_cost = 5,
                reincarnation_stage = 0,
            };
            var a2 = new UnitPhantasmCostRecord
            {
                id = 2,
                rare = 1,
                level_limitbreak = 1,
                phantasm_cost = 6,
                reincarnation_stage = 0,
            };
            var a3 = new UnitPhantasmCostRecord
            {
                id = 3,
                rare = 1,
                level_limitbreak = 2,
                phantasm_cost = 7,
                reincarnation_stage = 0,
            };
            var a4 = new UnitPhantasmCostRecord
            {
                id = 4,
                rare = 1,
                level_limitbreak = 3,
                phantasm_cost = 8,
                reincarnation_stage = 0,
            };
            var a5 = new UnitPhantasmCostRecord
            {
                id = 5,
                rare = 2,
                level_limitbreak = 0,
                phantasm_cost = 6,
                reincarnation_stage = 0,
            };
            var a6 = new UnitPhantasmCostRecord
            {
                id = 6,
                rare = 2,
                level_limitbreak = 1,
                phantasm_cost = 7,
                reincarnation_stage = 0,
            };
            var a7 = new UnitPhantasmCostRecord
            {
                id = 7,
                rare = 2,
                level_limitbreak = 2,
                phantasm_cost = 8,
                reincarnation_stage = 0,
            };
            var a8 = new UnitPhantasmCostRecord
            {
                id = 8,
                rare = 2,
                level_limitbreak = 3,
                phantasm_cost = 9,
                reincarnation_stage = 0,
            };
            var a9 = new UnitPhantasmCostRecord
            {
                id = 9,
                rare = 3,
                level_limitbreak = 0,
                phantasm_cost = 7,
                reincarnation_stage = 0,
            };
            var a10 = new UnitPhantasmCostRecord
            {
                id = 10,
                rare = 3,
                level_limitbreak = 1,
                phantasm_cost = 8,
                reincarnation_stage = 0,
            };
            var a11 = new UnitPhantasmCostRecord
            {
                id = 11,
                rare = 3,
                level_limitbreak = 2,
                phantasm_cost = 9,
                reincarnation_stage = 0,
            };
            var a12 = new UnitPhantasmCostRecord
            {
                id = 12,
                rare = 3,
                level_limitbreak = 3,
                phantasm_cost = 10,
                reincarnation_stage = 0,
            };
            var a13 = new UnitPhantasmCostRecord
            {
                id = 13,
                rare = 4,
                level_limitbreak = 0,
                phantasm_cost = 8,
                reincarnation_stage = 0,
            };
            var a14 = new UnitPhantasmCostRecord
            {
                id = 14,
                rare = 4,
                level_limitbreak = 1,
                phantasm_cost = 9,
                reincarnation_stage = 0,
            };
            var a15 = new UnitPhantasmCostRecord
            {
                id = 15,
                rare = 4,
                level_limitbreak = 2,
                phantasm_cost = 10,
                reincarnation_stage = 0,
            };
            var a16 = new UnitPhantasmCostRecord
            {
                id = 16,
                rare = 4,
                level_limitbreak = 3,
                phantasm_cost = 11,
                reincarnation_stage = 0,
            };
            var a17 = new UnitPhantasmCostRecord
            {
                id = 17,
                rare = 5,
                level_limitbreak = 0,
                phantasm_cost = 9,
                reincarnation_stage = 0,
            };
            var a18 = new UnitPhantasmCostRecord
            {
                id = 18,
                rare = 5,
                level_limitbreak = 1,
                phantasm_cost = 10,
                reincarnation_stage = 0,
            };
            var a19 = new UnitPhantasmCostRecord
            {
                id = 19,
                rare = 5,
                level_limitbreak = 2,
                phantasm_cost = 11,
                reincarnation_stage = 0,
            };
            var a20 = new UnitPhantasmCostRecord
            {
                id = 20,
                rare = 5,
                level_limitbreak = 3,
                phantasm_cost = 12,
                reincarnation_stage = 0,
            };
            var a21 = new UnitPhantasmCostRecord
            {
                id = 21,
                rare = 5,
                level_limitbreak = 3,
                phantasm_cost = 14,
                reincarnation_stage = 1,
            };
            var a22 = new UnitPhantasmCostRecord
            {
                id = 22,
                rare = 5,
                level_limitbreak = 3,
                phantasm_cost = 16,
                reincarnation_stage = 2,
            };
            var a23 = new UnitPhantasmCostRecord
            {
                id = 23,
                rare = 5,
                level_limitbreak = 3,
                phantasm_cost = 18,
                reincarnation_stage = 3,
            };
            var a24 = new UnitPhantasmCostRecord
            {
                id = 24,
                rare = 5,
                level_limitbreak = 3,
                phantasm_cost = 20,
                reincarnation_stage = 4,
            };
            var a25 = new UnitPhantasmCostRecord
            {
                id = 25,
                rare = 5,
                level_limitbreak = 3,
                phantasm_cost = 22,
                reincarnation_stage = 5,
            };




            UnitPhantasmCostTable t = new UnitPhantasmCostTable();
            t.m_table.Add(1, a1);
            t.m_table.Add(2, a2);
            t.m_table.Add(3, a3);
            t.m_table.Add(4, a4);
            t.m_table.Add(5, a5);
            t.m_table.Add(6, a6);
            t.m_table.Add(7, a7);
            t.m_table.Add(8, a8);
            t.m_table.Add(9, a9);
            t.m_table.Add(10, a10);
            t.m_table.Add(11, a11);
            t.m_table.Add(12, a12);
            t.m_table.Add(13, a13);
            t.m_table.Add(14, a14);
            t.m_table.Add(15, a15);
            t.m_table.Add(16, a16);
            t.m_table.Add(17, a17);
            t.m_table.Add(18, a18);
            t.m_table.Add(19, a19);
            t.m_table.Add(20, a20);
            t.m_table.Add(21, a21);
            t.m_table.Add(22, a22);
            t.m_table.Add(23, a23);
            t.m_table.Add(24, a24);
            t.m_table.Add(25, a25);

            //MessagePackSerializer.PrepareDictionaryType<long, UnitPhantasmCostRecord>();
            data = MasterMsgPack.m_context.GetSerializer<UnitPhantasmCostTable>().PackSingleObject(t);
            */

            /*
            var a1 = new TestRecord
            {
                id = 1,
                name = "追踪灵符",
                description = "def",
            };
            TestTable t = new TestTable();
            t.m_table.Add(1, a1);
            data = MasterMsgPack.m_context.GetSerializer<TestTable>().PackSingleObject(t);
            */

            var a1 = new ShotRecord
            {
                id = 10011,
                name = "追踪灵符",
                description = "\"博丽灵梦的扩散射击。\r\n灵妙的御札，会追踪对手的弹幕。\r\n虽然有点点犯规，但这也是巫女的实力。\"",
                specification = "\"[攻击]必中御符弹。\r\n[弹幕构成]偏阳气。\r\n[灵力强化追加]必中弹。\"",
                magazine0_bullet_id = 1,
                magazine0_bullet_range = 1,
                magazine0_bullet_value = 6,
                magazine0_bullet_power_rate = 482,
                magazine1_boost_count = 1,
                magazine1_bullet_id = 2,
                magazine1_bullet_range = 1,
                magazine1_bullet_value = 2,
                magazine1_bullet_power_rate = 363,
                magazine2_boost_count = 1,
                magazine2_bullet_id = 3,
                magazine2_bullet_range = 1,
                magazine2_bullet_value = 2,
                magazine2_bullet_power_rate = 333,
                magazine3_boost_count = 2,
                magazine3_bullet_id = 4,
                magazine3_bullet_range = 1,
                magazine3_bullet_value = 2,
                magazine3_bullet_power_rate = 286,
                magazine4_boost_count = 2,
                magazine4_bullet_id = 5,
                magazine4_bullet_range = 1,
                magazine4_bullet_value = 2,
                magazine4_bullet_power_rate = 257,
                magazine5_boost_count = 3,
                magazine5_bullet_id = 6,
                magazine5_bullet_range = 1,
                magazine5_bullet_value = 2,
                magazine5_bullet_power_rate = 229,
                phantasm_power_up_rate = 60,
                shot_level0_power_rate = 100,
                shot_level1_power_rate = 104,
                shot_level2_power_rate = 108,
                shot_level3_power_rate = 112,
                shot_level4_power_rate = 117,
                shot_level5_power_rate = 121,
            };
            var a2 = new ShotRecord
            {
                id = 10012,
                name = "追踪灵符",
                description = "\"博丽灵梦的扩散射击。\r\n灵妙的御札，会追踪对手的弹幕。\r\n虽然有点点犯规，但这也是巫女的实力。\"",
                specification = "\"[攻击]必中御符弹。\r\n[弹幕构成]偏阳气。\r\n[灵力强化追加]必中弹。\"",
                magazine0_bullet_id = 1,
                magazine0_bullet_range = 1,
                magazine0_bullet_value = 6,
                magazine0_bullet_power_rate = 482,
                magazine1_boost_count = 1,
                magazine1_bullet_id = 2,
                magazine1_bullet_range = 1,
                magazine1_bullet_value = 2,
                magazine1_bullet_power_rate = 363,
                magazine2_boost_count = 1,
                magazine2_bullet_id = 3,
                magazine2_bullet_range = 1,
                magazine2_bullet_value = 2,
                magazine2_bullet_power_rate = 333,
                magazine3_boost_count = 2,
                magazine3_bullet_id = 4,
                magazine3_bullet_range = 1,
                magazine3_bullet_value = 2,
                magazine3_bullet_power_rate = 286,
                magazine4_boost_count = 2,
                magazine4_bullet_id = 5,
                magazine4_bullet_range = 1,
                magazine4_bullet_value = 2,
                magazine4_bullet_power_rate = 257,
                magazine5_boost_count = 3,
                magazine5_bullet_id = 6,
                magazine5_bullet_range = 1,
                magazine5_bullet_value = 2,
                magazine5_bullet_power_rate = 229,
                phantasm_power_up_rate = 60,
                shot_level0_power_rate = 100,
                shot_level1_power_rate = 104,
                shot_level2_power_rate = 108,
                shot_level3_power_rate = 112,
                shot_level4_power_rate = 117,
                shot_level5_power_rate = 121,
            };
            var a3 = new ShotRecord
            {
                id = 10013,
                name = "追踪灵符",
                description = "\"博丽灵梦的扩散射击。\r\n灵妙的御札，会追踪对手的弹幕。\r\n虽然有点点犯规，但这也是巫女的实力。\"",
                specification = "\"[攻击]必中御符弹。\r\n[弹幕构成]偏阳气。\r\n[灵力强化追加]必中弹。\"",
                magazine0_bullet_id = 1,
                magazine0_bullet_range = 1,
                magazine0_bullet_value = 6,
                magazine0_bullet_power_rate = 482,
                magazine1_boost_count = 1,
                magazine1_bullet_id = 2,
                magazine1_bullet_range = 1,
                magazine1_bullet_value = 2,
                magazine1_bullet_power_rate = 363,
                magazine2_boost_count = 1,
                magazine2_bullet_id = 3,
                magazine2_bullet_range = 1,
                magazine2_bullet_value = 2,
                magazine2_bullet_power_rate = 333,
                magazine3_boost_count = 2,
                magazine3_bullet_id = 4,
                magazine3_bullet_range = 1,
                magazine3_bullet_value = 2,
                magazine3_bullet_power_rate = 286,
                magazine4_boost_count = 2,
                magazine4_bullet_id = 5,
                magazine4_bullet_range = 1,
                magazine4_bullet_value = 2,
                magazine4_bullet_power_rate = 257,
                magazine5_boost_count = 3,
                magazine5_bullet_id = 6,
                magazine5_bullet_range = 1,
                magazine5_bullet_value = 2,
                magazine5_bullet_power_rate = 229,
                phantasm_power_up_rate = 60,
                shot_level0_power_rate = 100,
                shot_level1_power_rate = 104,
                shot_level2_power_rate = 108,
                shot_level3_power_rate = 112,
                shot_level4_power_rate = 117,
                shot_level5_power_rate = 121,
            };
            ShotTable t = new ShotTable();
            t.m_table.Add(1, a1);
            t.m_table.Add(2, a2);
            t.m_table.Add(3, a3);
            data = MasterMsgPack.m_context.GetSerializer<ShotTable>().PackSingleObject(t);


            FileStream ofs = File.Create(Path.Combine(ROOT_DIR, Path.Combine(OUTPUT_DIR, "test")));
            ofs.Write(data, 0, data.Length);
        }
    }
}
