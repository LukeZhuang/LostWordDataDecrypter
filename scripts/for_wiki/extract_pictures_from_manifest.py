import os
import json
import re
import wget

DOWNLOAD_PREFIX = "https://d3s38hlip7moa.cloudfront.net/assetbundle/android/20230724_111831/qQiFjgaz/"
OUTPUT_DIR = "./output"

manifest_file = "manifest_jp_20230724.json"

f = open(manifest_file)
pic_list = open("missing_picture_num_list.txt")
missing_pic = set([str(int(line.strip())) for line in pic_list])

pattern = r'^Assets/East/Pictures/([0-9]+)'

data = json.load(f)
asset_infos = data['AssetInfos']

download_list = []

for asset_info in asset_infos:
	if "Pictures" not in str(asset_info):
		continue
	file_name = asset_info["Name"]
	asset_paths = asset_info["AssetPaths"]
	pic_id = None
	for asset_path in asset_paths:
		g = re.findall(pattern, asset_path)
		assert len(g) == 1
		assert pic_id == None or pic_id == g[0]
		pic_id = g[0]
	if pic_id != None:
		if pic_id in missing_pic:
			print(pic_id, file_name)
			download_list.append(file_name)

for file_name in download_list:
	print("downloading:", file_name)
	url = DOWNLOAD_PREFIX + file_name
	output_path = OUTPUT_DIR
	output = wget.download(url, output_path)
