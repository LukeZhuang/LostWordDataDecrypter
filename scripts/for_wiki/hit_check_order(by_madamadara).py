import json
import urllib.request
import os
import UnityPy
import re
from tqdm import tqdm

url_asset = "https://thcdn.gggamedownload.com/source/Assetbundle_Android_v"
data_path = 'ori_data/'
target_path = '角色/'
version = 3063

os.chdir(os.path.pardir)
order_dict = {}
f = open(data_path + 'UnitMaster.json', 'r', encoding='utf-8')
Unit_data = json.load(f)
f.close()
for key in Unit_data:
    if Unit_data[key][2] != '' and Unit_data[key][3] != '_TEST' and key != '1041':
        order_dict[int(key)] = ['1符 ', '2符 ', '终符 ']

url = url_asset + str(version) + '/'
manifest = urllib.request.urlopen(url + 'manifest.json').read()
f = open(target_path + 'manifest.json', 'wb')
f.write(manifest)
f.close()
data_infos = json.loads(manifest)['AssetInfos']
for data_info in tqdm(data_infos):
    asset_paths = data_info['AssetPaths']
    temp_flag = 0
    for asset_path in asset_paths:
        if 'Timeline/Barrage' in asset_path:
            temp_flag = 1
    if temp_flag != 0:
        # 下载资源
        asset_name = data_info['Name']
        asset_data = urllib.request.urlopen(url + asset_name).read()
        # 正则提取id
        pattern = r'Units/(.*)/Timeline'
        unit_id = re.findall(pattern, asset_paths[0])[0]
        # 获取asset
        bundle = UnityPy.load(asset_data)
        # 解包asset
        objects = bundle.assets[0].objects
        hit_dict = {}
        card_flag = 0
        for key in objects:
            obj = objects[key]
            if 'MonoBehaviour' in str(obj.type):
                game_object = obj.read()
                if hasattr(game_object, 'm_mgznid'):
                    hit_dict[game_object.path_id] = game_object.m_mgznid + 1
        for key in objects:
            card_flag = 0
            obj = objects[key]
            if 'MonoBehaviour' in str(obj.type):
                game_object = obj.read()
                #  判断是哪张符卡
                if game_object.name == 'Barrage33':
                    card_flag = 0
                elif game_object.name == 'Barrage43':
                    card_flag = 1
                elif game_object.name == 'Barrage73':
                    card_flag = 2
                else:
                    continue
                for order in game_object.m_ordrlist:
                    if order.path_id in hit_dict and int(unit_id) in order_dict:
                        order_dict[int(unit_id)][card_flag] += str(hit_dict[order.path_id])
os.remove(target_path + 'manifest.json')

text = ''
for key in order_dict:
    for i in range(3):
        text += str(key) + ' ' + order_dict[key][i] + '\n'
with open(target_path + '段落顺序.txt', 'w', encoding='utf-8') as f:
    f.write(text)
