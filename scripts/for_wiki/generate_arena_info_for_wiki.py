import csv
import os

f = open('output.txt', 'w')
arena_enemie_dict = {}
units = {}
resists = {}
skill_effects = {}
skills = {}
chinese_numbers = ["一", "二", "三", "四", "五", "六", "七"]
DATA_DIR = "../../decrypt_output"
elements = ["日","月","火","水","木","金","土","星"]

with open(os.path.join(DATA_DIR, "ResistTable.csv")) as csvfile:
	reader = csv.DictReader(csvfile)
	for row in reader:
		weak = ""
		resist = ""
		for ele in range(8):
			k = int(row["element" + str(ele+1) + "_resistance"])
			if k == 0:
				weak += elements[ele]
			elif k == 2:
				resist += elements[ele]
		resists[row["id"]] = (weak, resist)

with open(os.path.join(DATA_DIR, "UnitTable.csv")) as csvfile:
	reader = csv.DictReader(csvfile)
	for row in reader:
		units[row["id"]] = row["name"] + row["symbol_name"]

with open(os.path.join(DATA_DIR, "SkillEffectTable.csv")) as csvfile:
	reader = csv.DictReader(csvfile)
	for row in reader:
		skill_effects[row["id"]] = row["description"]

with open(os.path.join(DATA_DIR, "SkillTable.csv")) as csvfile:
	reader = csv.DictReader(csvfile)
	for row in reader:
		effect_strs = []
		for effect in range(3):
			if row["effect" + str(effect+1) + "_id"] == "0":
				continue
			effect_strs.append(skill_effects[row["effect" + str(effect+1) + "_id"]])
		skills[row["id"]] = "，".join(effect_strs)

with open(os.path.join(DATA_DIR, "ArenaEnemyTable.csv")) as csvfile:
	reader = csv.DictReader(csvfile)
	for row in reader:
		if int(row["id"]) >= 200001 and int(row["id"]) < 300000:
			skill_strs = []
			for skill in range(3):
				if row["skill" + str(skill+1) + "_id"] == "0":
					continue
				skill_strs.append(skills[row["skill" + str(skill+1) + "_id"]])
			barrier_str = chinese_numbers[int(row["barrier_count"]) - 1] + "盾"
			weak, resist = resists[row["resist_id"]]
			arena_enemie_dict[int(row["unit_id"])] = (units[row["unit_id"]], barrier_str, weak, resist, "。".join(skill_strs) + "。")


def replace_color(skill):
	skill = skill.replace("<color", "<font color")
	skill = skill.replace("</color>", "</font>")
	return skill

date_info = "周擂台2023年9月11日至2023年9月17日"
arena_types = [['回复','干扰'],['防御','技巧'],['攻击','速攻'],['支援','破坏']]
arena_enemies = [
  [  # arena1
    [1023], # stage1
    [1070], # stage2
    [1073,1046,1053], # stage3
  ],
  [  # arena2
    [1058,1039], # stage1
    [1101,4001,1008], # stage2
    [1067], # stage3
  ],
  [  # arena3
    [1006,1009], # stage1
    [1100], # stage2
    [1114,1071], # stage3
  ],
  [  # arena4
    [1120], # stage1
    [1018,1011], # stage2
    [1022,1033,1112], # stage3
  ],
]


f.write("<div style=\"overflow:auto\">\n")
f.write(date_info + "\n")
f.write("""\
{| class="wikitable" style="text-align:center;width:100%"
|-
|角色
|头像
|盾数
|弱点
|耐性
|技能
""")
for (index, arena_type) in enumerate(arena_types):
	type1, type2 = arena_type
	f.write("|-\n")
	f.write("!colspan=6|<font color=red>" + type1 + "/" + type2 + ("阳" if index % 2 == 0 else "阴") + "擂台</font>\n")
	enemies = arena_enemies[index]
	for stage in range(3):
		f.write("|-\n")
		f.write("!colspan=6|第" + chinese_numbers[stage] + "面\n")
		stage_enemies = enemies[stage]
		for enemy_id in stage_enemies:
			f.write("|-\n")
			name, barrier_str, weak, resist, skill = arena_enemie_dict[enemy_id]
			f.write("|" + name + "\n")
			f.write("|[[文件:S" + str(enemy_id) + "01.png|25px|link=]]\n")
			f.write("|" + barrier_str + "\n")
			f.write("|" + "<font color=\"red\">" + weak + "</font>\n")
			f.write("|" + "<font color=\"blue\">" + resist + "</font>\n")
			f.write("|" + replace_color(skill) + "\n")
f.write("""|}
</div>""")

f.write("""

{{文章戳
|文章上级页面=周擂台
|时间=
|作者=
|是否原创=是
|来源=
|原文地址=
}}

[[分类:周擂台]]
""")
