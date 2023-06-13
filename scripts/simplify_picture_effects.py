race_str = {}
frt = open('./RaceTable.csv')
for line in frt:
	if line.startswith('id'):
		continue
	fields = line.split(',')
	race_str[int(fields[0])] = fields[1]
frt.close()


def range_str(range):
	if range == 0 or range == 1:
		return '自身'
	elif range == 2:
		return '己全'
	elif range == 3:
		return '敌单'
	elif range == 4:
		return '敌全'
	else:
		raise Exception('not implemented')

def buffdebuff(effect_type):
	if effect_type == 1 or (effect_type >= 21 and effect_type <= 28):
		return 1
	elif effect_type == 2 or (effect_type >= 31 and effect_type <= 38):
		return -1
	return 0

def effect_str(effect_subtype):
	if effect_subtype == 1:
		return '阳攻'
	elif effect_subtype == 2:
		return '阳防'
	elif effect_subtype == 3:
		return '阴攻'
	elif effect_subtype == 4:
		return '阴防'
	elif effect_subtype == 5:
		return '速度'
	elif effect_subtype == 6:
		return '命中'
	elif effect_subtype == 7:
		return '回避'
	elif effect_subtype == 8:
		return '会心攻击'
	elif effect_subtype == 9:
		return '会心防御'
	elif effect_subtype == 10:
		return '会心命中'
	elif effect_subtype == 11:
		return '会心回避'
	elif effect_subtype == 12:
		return '仇恨值'

def role_prefix(effect_type):
	role = None
	if (effect_type >= 21 and effect_type <= 28):
		role = effect_type - 20
	elif (effect_type >= 31 and effect_type <= 38):
		role = effect_type - 30
	else:
		return ''
	if role == 1:
		return '防御式'
	elif role == 2:
		return '支援式'
	elif role == 3:
		return '回复式'
	elif role == 4:
		return '干扰式'
	elif role == 5:
		return '攻击式'
	elif role == 6:
		return '技巧式'
	elif role == 7:
		return '速攻式'
	elif role == 8:
		return '破坏式'
	else:
		raise Exception('not implemented')

def bullet_type_str(effect_subtype):
	if effect_subtype == 1:
		return '通常弹'
	elif effect_subtype == 2:
		return '镭射弹'
	elif effect_subtype == 3:
		return '体术弹'
	elif effect_subtype == 4:
		return '斩击弹'
	elif effect_subtype == 5:
		return '动能弹'
	elif effect_subtype == 6:
		return '流体弹'
	elif effect_subtype == 7:
		return '能量弹'
	elif effect_subtype == 8:
		return '御符弹'
	elif effect_subtype == 9:
		return '光弹'
	elif effect_subtype == 10:
		return '尖头弹'
	elif effect_subtype == 11:
		return '追踪弹'
	else:
		raise Exception('not implemented')

def element_type_str(effect_subtype):
	if effect_subtype == 1:
		return '日属性'
	elif effect_subtype == 2:
		return '月属性'
	elif effect_subtype == 3:
		return '火属性'
	elif effect_subtype == 4:
		return '水属性'
	elif effect_subtype == 5:
		return '木属性'
	elif effect_subtype == 6:
		return '金属性'
	elif effect_subtype == 7:
		return '土属性'
	elif effect_subtype == 8:
		return '星属性'
	elif effect_subtype == 9:
		return '无属性'
	else:
		raise Exception('not implemented')

def simplified_str(effect_type, effect_subtype, effect_val, effect_turn, effect_range):
	if effect_type == 0:
		return ''
	elif buffdebuff(effect_type) != 0:
		return role_prefix(effect_type) + range_str(effect_range) + effect_str(effect_subtype) + ('+' if buffdebuff(effect_type) == 1 else '-') + str(effect_val) + '级(' + str(effect_turn) + 't)'
	elif effect_type == 3:
		return range_str(effect_range) + '回' + str(effect_val) + '%血'
	elif effect_type == 4:
		return range_str(effect_range) + '+' + str(effect_val) + '盾'
	elif effect_type == 5:
		return range_str(effect_range) + '+' + str(effect_val/20) + 'P'
	elif effect_type == 12:
		return range_str(effect_range) + '受到' + bullet_type_str(effect_subtype) + '-' + str(effect_val) + '%'
	elif effect_type == 13:
		return range_str(effect_range) + '受到' + element_type_str(effect_subtype) + '-' + str(effect_val) + '%'
	elif effect_type == 14:
		return range_str(effect_range) + '受到' + race_str[effect_subtype] + '-' + str(effect_val) + '%'
	elif effect_type == 15:
		return bullet_type_str(effect_subtype) + '+' + str(effect_val) + '%'
	elif effect_type == 16:
		return element_type_str(effect_subtype) + '+' + str(effect_val) + '%'
	elif effect_type == 17:
		return '灵力回收效率+' + str(effect_val) + '%'
	else:
		print(effect_type)
		raise Exception('not implemented')


def solve(line):
	print(line)
	fields = line.split(',')
	simplified_chara_str = []
	simplified_chara_max_str = []
	for chara_id in range(3):
		col = 18 + chara_id * 6
		effect_type = int(fields[col])
		effect_subtype = int(fields[col + 1])
		effect_val = int(fields[col + 2])
		effect_val_max = int(fields[col + 3])
		effect_turn = int(fields[col + 4])
		effect_range = int(fields[col + 5])

		eff1_str = simplified_str(effect_type, effect_subtype, effect_val, effect_turn, effect_range)
		eff1_max_str = simplified_str(effect_type, effect_subtype, effect_val_max, effect_turn, effect_range)
		simplified_chara_str.append(eff1_str)
		simplified_chara_max_str.append(eff1_max_str)
	print(simplified_chara_str)
	print(simplified_chara_max_str)
	return simplified_chara_str, simplified_chara_max_str




f = open('./PictureTable.csv')
for line in f:
	if line.startswith('id'):
		continue
	solve(line)
f.close()
