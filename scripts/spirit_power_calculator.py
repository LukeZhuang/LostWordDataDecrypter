import csv
import math
import numpy as np
import random

fo = open('output.txt', 'w')

MONTE_CARLO = 10000

bullet_dict = {}
shot_dict = {}
spellcard_dict = {}
unit_dict = {}


def bullet_is_sure_hit(bullet):
	return '1' in [bullet["bullet" + str(i+1) + "_addon_id"] for i in range(3)]


def generate_magazine_info(shot):
	boosts = [[], [], [], []]
	shot_range = None
	for magazine_id in range(6):
		prefix = 'magazine' + str(magazine_id) + '_'
		boost_id = int(shot[prefix + 'boost_count']) if (magazine_id > 0) else 0
		prefix += 'bullet_'
		if shot[prefix + 'id'] != '0':
			bullet_info = bullet_dict[shot[prefix + 'id']]
			if shot_range is None:
				shot_range = int(shot[prefix + 'range'])
			boosts[boost_id].append((int(shot[prefix + 'value']), bullet_info[0], bullet_info[1]))
	return (int(shot['phantasm_power_up_rate']), shot_range, boosts)


def is_player_unit(unit):
	return unit['symbol_name'] != '(empty)' and (unit['shot1_id'] != '10011' or unit['id'] == '1001') and unit['alias_name'] != '_TEST'


def get_hit_rank_rate(hit_rank):
	assert hit_rank >= -10 and hit_rank <= 10
	rate = 1.0 + 0.2 * abs(hit_rank)
	if hit_rank < 0:
		rate = 1.0 / rate
	return rate


def get_actual_hit(bullet_hit, hit_rank):
	updated_bullet_hit = bullet_hit * get_hit_rank_rate(hit_rank)
	updated_bullet_hit = max(0.0, min(100.0, updated_bullet_hit)) / 100.0
	return updated_bullet_hit


# def one_time_bullet_phantasm_collect(phantasm_power_up_rate, is_sure_hit, bullet_hit, hit_rank):
# 	if is_sure_hit:
# 		hit_succ = True
# 	else:
# 		updated_bullet_hit = bullet_hit * get_hit_rank_rate(hit_rank)
# 		updated_bullet_hit = max(0.0, min(100.0, updated_bullet_hit)) / 100.0
# 		hit_succ = (random.random() <= updated_bullet_hit)

# 	if hit_succ:
# 		return math.floor((phantasm_power_up_rate / 100.0) * 4.0 * random.randint(3, 7)) / 100.0
# 	return 0.0


# def one_time_shot_phantasm_collect(shot_info, enemy_count, hit_rank):
# 	phantasm_power_up_rate, shot_range, boosts = shot_info
# 	actual_enemy_count = 1 if shot_range == 1 else enemy_count
# 	collect = []
# 	accumulate_collect = 0.0
# 	for boost_count in range(4):
# 		cur_boost_collect = 0.0
# 		for bullet_info in boosts[boost_count]:
# 			bullet_value, bullet_hit, is_sure_hit = bullet_info
# 			cur_bullet_collect = 0.0
# 			for i in range(bullet_value * actual_enemy_count):
# 				cur_bullet_collect += one_time_bullet_phantasm_collect(phantasm_power_up_rate, is_sure_hit, bullet_hit, hit_rank)
# 			cur_boost_collect += cur_bullet_collect
# 		accumulate_collect += cur_boost_collect
# 		collect.append(round(accumulate_collect, 2))
# 	return collect


# def analyze_collects(collects):
# 	info = []
# 	for boost_count in range(4):
# 		cur_boost_collect = sorted([collect[boost_count] for collect in collects])
# 		avg = round(sum(cur_boost_collect) / len(cur_boost_collect), 2)
# 		greater_than_90 = round(cur_boost_collect[int(len(cur_boost_collect) * (1 - 0.9))], 2)
# 		greater_than_95 = round(cur_boost_collect[int(len(cur_boost_collect) * (1 - 0.95))], 2)
# 		greater_than_99 = round(cur_boost_collect[int(len(cur_boost_collect) * (1 - 0.99))], 2)
# 		info.append((avg, greater_than_90, greater_than_95, greater_than_99))
# 	return info



# def shot_collect(shot_info):
# 	_, shot_range, __ = shot_info
# 	# enemy_counts = list(range(1, 4)) if shot_range != 1 else [1]
# 	# hit_ranks = list(range(-10, 11))
# 	# hit_ranks = list(range(-5, 6))
# 	enemy_counts = [1]
# 	hit_ranks = [0]
# 	for hit_rank in hit_ranks:
# 		for enemy_count in enemy_counts:
# 			collects = []
# 			for i in range(MONTE_CARLO):
# 				collects.append(one_time_shot_phantasm_collect(shot_info, enemy_count, hit_rank))
# 			collects_info = analyze_collects(collects)
# 			fo.write(f"{enemy_count}, {hit_rank}, {collects_info}\n")


def numpy_shot_collect(shot_info):
	phantasm_power_up_rate, shot_range, boosts = shot_info
	# enemy_counts = [1]
	enemy_counts = [3] if shot_range != 1 else [1]
	hit_ranks = [0]
	# enemy_counts = list(range(1, 4)) if shot_range != 1 else [1]
	# hit_ranks = list(range(-10, 11))
	for hit_rank in hit_ranks:
		for enemy_count in enemy_counts:
			accumulate_sp = np.zeros(MONTE_CARLO)
			collects_info = []
			for cur_boost in boosts:
				for bullet_info in cur_boost:
					bullet_value, bullet_hit, is_sure_hit = bullet_info
					tot_bullets = bullet_value * enemy_count
					sp = np.floor(phantasm_power_up_rate * np.random.randint(3, 8, (tot_bullets, MONTE_CARLO)) * 0.04) * 0.01
					if not is_sure_hit:
						hit_mask = np.random.rand(tot_bullets, MONTE_CARLO) < get_actual_hit(bullet_hit, hit_rank)
						sp = sp * hit_mask
					magazine_sp = np.sum(sp, axis=0)
					accumulate_sp += magazine_sp
				tmp = np.sort(np.copy(accumulate_sp))
				avg = np.average(tmp)
				greater_than_90 = tmp[int(MONTE_CARLO * (1 - 0.90))]
				greater_than_95 = tmp[int(MONTE_CARLO * (1 - 0.95))]
				greater_than_99 = tmp[int(MONTE_CARLO * (1 - 0.99))]
				greater_than_999 = tmp[int(MONTE_CARLO * (1 - 0.999))]
				# l = [avg, greater_than_90, greater_than_95, greater_than_99, greater_than_999]
				l = [avg, greater_than_90, greater_than_95, greater_than_99]
				l = [round(f, 2) for f in l]
				collects_info.append(l)
			fo.write(f"{enemy_count}, {hit_rank}, {collects_info}\n")



print('loading bullet table...')
with open('../decrypt_output/BulletTable.csv', 'r') as bullet_table_file:
	bullet_records = csv.DictReader(bullet_table_file)
	for bullet in bullet_records:
		bullet_dict[bullet["id"]] = (int(bullet['hit']), bullet_is_sure_hit(bullet))


print('loading shot table...')
with open('../decrypt_output/ShotTable.csv', 'r') as shot_table_file:
	shot_records = csv.DictReader(shot_table_file)
	for shot in shot_records:
		shot_dict[shot["id"]] = generate_magazine_info(shot)


print('loading spellcard table...')
with open('../decrypt_output/SpellcardTable.csv', 'r') as spellcard_table_file:
	spellcard_records = csv.DictReader(spellcard_table_file)
	for spellcard in spellcard_records:
		spellcard_dict[spellcard["id"]] = generate_magazine_info(spellcard)


print('loading unit table...')
with open('../decrypt_output/UnitTable.csv', 'r') as unit_table_file:
	unit_records = csv.DictReader(unit_table_file)
	for unit in unit_records:
		if is_player_unit(unit):
			unit_dict[unit["id"]] = unit


# for unit_id, unit in unit_dict.items():
for unit_id, unit in [("1001", unit_dict["1001"])]:
	fo.write(unit_id + unit["name"] + unit["symbol_name"] + "\n")
	for i in range(1, 3):
		shot_name = "shot" + str(i) + "_id"
		fo.write(shot_name + "\n")
		# shot_collect(shot_dict[unit[shot_name]])
		numpy_shot_collect(shot_dict[unit[shot_name]])
	for i in [1, 2, 5]:
		spellcard_name = "spellcard" + str(i) + "_id"
		fo.write(spellcard_name + "\n")
		# shot_collect(spellcard_dict[unit[spellcard_name]])
		numpy_shot_collect(spellcard_dict[unit[spellcard_name]])
