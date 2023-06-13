import os
import shutil
import sys

ORIGIN_DIR = './output'
OUTPUT_DIR = './output_renamed'
# FILE_DIR = '../bin/Debug/output_files/ch05182023'
FILE_DIR = '.'
PICTURE_FILE = 'PictureTable.csv'
UNIT_FILE = 'UnitTable.csv'

pic_names = {}
fpic = open(os.path.join(FILE_DIR, PICTURE_FILE))
for line in fpic:
	if line.startswith('id'):
		continue
	fields = line.strip().split(',')
	pic_names[fields[0]] = fields[1]
fpic.close()

unit_names = {}
funit = open(os.path.join(FILE_DIR, UNIT_FILE))
for line in funit:
	if line.startswith('id'):
		continue
	fields = line.strip().split(',')
	unit_names[fields[0]] = fields[1] + fields[9]
funit.close()

for dirs in os.listdir(ORIGIN_DIR):
	output_path = os.path.join(OUTPUT_DIR, dirs)
	if not os.path.exists(output_path):
		os.makedirs(output_path)
	fn = os.path.join(ORIGIN_DIR, dirs)
	if dirs.startswith('picture'):
		for pic in os.listdir(fn):
			assert pic.endswith('.png')
			pic_name = pic[:-4]
			if pic_name in pic_names:
				# print(pic_name, pic_names[pic_name])
				shutil.copy(os.path.join(fn, pic), os.path.join(output_path, pic_name + "-" + pic_names[pic_name] + ".png"))
	elif dirs.startswith('unit'):
		for unit in os.listdir(fn):
			assert unit.endswith('.png')
			unit_name = unit[:-4]
			if unit_name in unit_names:
				# print(unit_name, unit_names[unit_name])
				shutil.copy(os.path.join(fn, unit), os.path.join(output_path, unit_name + "-" + unit_names[unit_name] + ".png"))
	else:
		assert False