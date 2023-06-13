import os

dir = '../decrypt_output'

f1 = open(os.path.join(dir, 'EpisodeTable.csv'))
f2 = open(os.path.join(dir, 'BattleTable.csv'))
f3 = open(os.path.join(dir, 'BattleDropTable.csv'))
f4 = open(os.path.join(dir, 'PictureTable.csv'))

picture_table = {}
for line in f4:
	if line.startswith('id'):
		continue
	fields = line.strip().split(',')
	picture_table[int(fields[0])] = fields[1]

drop_table = {}
for line in f3:
	if line.startswith('id'):
		continue
	fields = line.strip().split(',')
	if fields[2] == '10':
		drop_table[int(fields[1])] = int(fields[3])

battle_table = {}
for line in f2:
	if line.startswith('id'):
		continue
	fields = line.strip().split(',')
	battle_id = int(fields[0])
	drop_ids = [int(fields[20]), int(fields[21]), int(fields[22]), int(fields[23])]
	battle_table[battle_id] = drop_ids


for line in f1:
	if line.startswith('id'):
		continue
	fields = line.strip().split(',')
	episode_id = int(fields[0])
	title = fields[3]
	normal_battle = int(fields[7])
	hard_battle = int(fields[8])
	lunatic_battle = int(fields[9])

	if normal_battle in battle_table:
		normal_drops = battle_table[normal_battle]
		for normal_drop in normal_drops:
			if normal_drop in drop_table:
				assert drop_table[normal_drop] in picture_table
				print(title, 'normal', picture_table[drop_table[normal_drop]])

	if hard_battle in battle_table:
		hard_drops = battle_table[hard_battle]
		for hard_drop in hard_drops:
			if hard_drop in drop_table:
				assert drop_table[hard_drop] in picture_table
				print(title, 'hard', picture_table[drop_table[hard_drop]])

	if lunatic_battle in battle_table:
		lunatic_drops = battle_table[lunatic_battle]
		for lunatic_drop in lunatic_drops:
			if lunatic_drop in drop_table:
				assert drop_table[lunatic_drop] in picture_table
				print(title, 'lunatic', picture_table[drop_table[lunatic_drop]])
