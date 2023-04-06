import os
import shutil
import sys

from pathlib import Path

asset_path = sys.argv[1]
pictures_img = {}
units_img = {}

OUTPUT_DIR = './output'

output_path = "pictures_img"
output_path = os.path.join(OUTPUT_DIR, output_path)
if not os.path.exists(output_path):
	os.makedirs(output_path)

print('searching for pictures images...')
for path in Path(asset_path).rglob('pictures'):
	print('found a pictures directory:', path)
	for pic_name in os.listdir(path):
		fn = os.path.join(path, pic_name)
		if os.path.isdir(fn):
			for img_path in Path(fn).rglob('ThumbSquare.png'):
				print('find:', img_path)
				shutil.copy(img_path, os.path.join(output_path, pic_name + ".png"))


output_path = "units_img"
output_path = os.path.join(OUTPUT_DIR, output_path)
if not os.path.exists(output_path):
	os.makedirs(output_path)

for path in Path(asset_path).rglob('units'):
	print('found a units directory:', path)
	for unit_name in os.listdir(path):
		fn = os.path.join(path, unit_name)
		fn = os.path.join(fn, '01')
		if os.path.isdir(fn):
			for img_path in Path(fn).rglob('Square.png'):
				print('find:', img_path)
				shutil.copy(img_path, os.path.join(output_path, unit_name + ".png"))