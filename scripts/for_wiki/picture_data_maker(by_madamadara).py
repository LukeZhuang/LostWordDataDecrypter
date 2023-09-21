import pandas as pd
import json
import csv

data_path = 'PictureTabeDiff.csv'
target_path = './'
source_path = "PictureSource.csv"

picture_source = {}

with open(source_path) as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader:
        picture_source[row["id"]] = row["source"]

range_dict = {0: '自身', 1: '自身', 2: '己方全体', 3: '敌方单体', 4: '敌方全体'}
character_dict = {21: '防御式', 31: '防御式', 22: '支援式', 32: '支援式', 23: '回复式', 33: '回复式', 24: '干扰式', 34: '干扰式', 25: '攻击式',
                  35: '攻击式', 26: '技巧式', 36: '技巧式', 27: '速攻式', 37: '速攻式', 28: '破坏式', 38: '破坏式', }
buff_dict = {1: '阳攻', 2: '阳防', 3: '阴攻', 4: '阴防', 5: '速度', 6: '命中', 7: '回避', 8: '会心攻击', 9: '会心防御', 10: '会心命中',
             11: '会心回避',
             12: '仇恨值'}
bullet_dict = {1: '通常弹', 2: '镭射弹', 3: '体术弹', 4: '斩击弹', 5: '动能弹', 6: '流体弹', 7: '能量弹', 8: '御符弹', 9: '光弹', 10: '尖头弹',
               11: '追踪弹'}
element_dict = {1: '日', 2: '月', 3: '火', 4: '水', 5: '木', 6: '金', 7: '土', 8: '星', 9: '无'}
other_dict = {3: '体力回复', 4: '结界回复', 5: '灵力上升', 17: '灵力回收效率'}
picture_type_dict = {1: '梅', 2: '兰', 3: '菊', 4: '竹'}
correction_type_dict = {1: '体力', 2: '阳攻', 3: '阳防', 4: '阴攻', 5: '阴防', 6: '速度'}

pd_all = pd.read_csv(data_path)

name_list = pd_all.name
id_list = pd_all.id
type_ids = pd_all.type
type_list = []
for type_id in type_ids:
    type_list.append(picture_type_dict[type_id])
rare_list = pd_all.rare
illustrator_list = pd_all.illustrator_name

# buff类型、debuff类型、作用范围、弹种、特性属性、其他效果、角色式限定
characteristic1_effect_type_list = pd_all.picture_characteristic1_effect_type
characteristic2_effect_type_list = pd_all.picture_characteristic2_effect_type
characteristic3_effect_type_list = pd_all.picture_characteristic3_effect_type
characteristic1_effect_subtype_list = pd_all.picture_characteristic1_effect_subtype
characteristic2_effect_subtype_list = pd_all.picture_characteristic2_effect_subtype
characteristic3_effect_subtype_list = pd_all.picture_characteristic3_effect_subtype
characteristic1_effect_range_list = pd_all.picture_characteristic1_effect_range
characteristic2_effect_range_list = pd_all.picture_characteristic2_effect_range
characteristic3_effect_range_list = pd_all.picture_characteristic3_effect_range

buff_list = []
debuff_list = []
range_list = []
bullet_type_list = []
element_type_list = []
other_effect_list = []
character_type_list = []
for i in range(len(name_list)):
    _buff = ''
    _debuff = ''
    _range = ''
    _bullet_type = ''
    _element_type = ''
    _other_effect = ''
    _character_type = ''
    effects = [characteristic1_effect_type_list[i], characteristic2_effect_type_list[i],
               characteristic3_effect_type_list[i]]
    subtypes = [characteristic1_effect_subtype_list[i], characteristic2_effect_subtype_list[i],
                characteristic3_effect_subtype_list[i]]
    ranges = [characteristic1_effect_range_list[i], characteristic2_effect_range_list[i],
              characteristic3_effect_range_list[i]]
    for j in range(3):
        if int(effects[j]) in [1, 21, 22, 23, 24, 25, 26, 27, 28]:
            _buff = _buff + buff_dict[int(subtypes[j])] + '、'
        if int(effects[j]) in [2, 31, 32, 33, 34, 35, 36, 37, 38]:
            _debuff = _debuff + buff_dict[int(subtypes[j])] + '、'
        if int(effects[j]) in [3, 4, 5, 17]:
            _other_effect = _other_effect + other_dict[int(effects[j])] + '、'
        if int(effects[j]) in [15]:
            _bullet_type = _bullet_type + bullet_dict[int(subtypes[j])] + '、'
        if int(effects[j]) in [16]:
            _element_type = _element_type + element_dict[int(subtypes[j])] + '、'
        if int(effects[j]) in [21, 22, 23, 24, 25, 26, 27, 28, 31, 32, 33, 34, 35, 36, 37, 38]:
            _character_type = _character_type + character_dict[int(effects[j])] + '、'
        _range = _range + range_dict[int(ranges[j])] + '、'
    buff_list.append(_buff)
    debuff_list.append(_debuff)
    range_list.append(_range)
    bullet_type_list.append(_bullet_type)
    element_type_list.append(_element_type)
    other_effect_list.append(_other_effect)
    character_type_list.append(_character_type)


correction1_type_ids = pd_all.correction1_type
correction1_type_list = []
for type_id in correction1_type_ids:
    correction1_type_list.append(correction_type_dict[type_id])
correction2_type_ids = pd_all.correction2_type
correction2_type_list = []
for type_id in correction2_type_ids:
    correction2_type_list.append(correction_type_dict[type_id])
correction1_value0_list = []
correction1_value10_list = pd_all.correction1_value
correction1_value_diff_list = pd_all.correction1_diff
for i in range(len(correction1_value10_list)):
    correction1_value0_list.append(correction1_value10_list[i] - 10 * correction1_value_diff_list[i])
correction2_value0_list = []
correction2_value10_list = pd_all.correction2_value
correction2_value_diff_list = pd_all.correction2_diff
for i in range(len(correction2_value10_list)):
    correction2_value0_list.append(correction2_value10_list[i] - 10 * correction2_value_diff_list[i])
# 获取途径
# source = pd_all.source

characteristic_list_ori = pd_all.picture_characteristic_text
characteristic_list = []
# 处理文本格式
for text_ori in characteristic_list_ori:
    text = text_ori.replace('<color=', '<font color=').replace('</color>', '</font>').replace('<ret>', '<br>')
    characteristic_list.append(text)

characteristic_max_list_ori = pd_all.picture_characteristic_text_max
characteristic_max_list = []
for text_ori in characteristic_max_list_ori:
    text = text_ori.replace('<color=', '<font color=').replace('</color>', '</font>').replace('<ret>', '<br>')
    characteristic_max_list.append(text)

flavor_text1_list = pd_all.flavor_text1
flavor_text2_list = pd_all.flavor_text2
flavor_text3_list = pd_all.flavor_text3
flavor_text4_list = pd_all.flavor_text4
flavor_text5_list = pd_all.flavor_text5
# TODO 特性简缩

# 6维
six_value_list = [[], [], [], [], [], []]
for i in range(len(correction1_value10_list)):
    six_value_list[0].append('')
    six_value_list[1].append('')
    six_value_list[2].append('')
    six_value_list[3].append('')
    six_value_list[4].append('')
    six_value_list[5].append('')
    six_value_list[correction1_type_ids[i] - 1][i] = correction1_value10_list[i]
    six_value_list[correction2_type_ids[i] - 1][i] = correction2_value10_list[i]

pictures_json = {}
for i in range(len(name_list)):
    picture_dict = {
        "名称": name_list[i],
        "编号": str(id_list[i]),
        "类型": type_list[i],
        "稀有度": str(rare_list[i]),
        "画师": illustrator_list[i],
        "buff类型": buff_list[i],
        "debuff类型": debuff_list[i],
        "作用范围": range_list[i],
        "弹种": bullet_type_list[i],
        "特性属性": element_type_list[i],
        "其他效果": other_effect_list[i],
        "角色式限定": character_type_list[i],
        "查询属性": correction1_type_list[i] + '、' + correction2_type_list[i],
        "属性1": correction1_type_list[i],
        "属性2": correction2_type_list[i],
        "0级属性1": str(correction1_value0_list[i]),
        "0级属性2": str(correction2_value0_list[i]),
        "10级属性1": str(correction1_value10_list[i]),
        "10级属性2": str(correction2_value10_list[i]),
        "体力": str(six_value_list[0][i]),
        "阳攻": str(six_value_list[1][i]),
        "阳防": str(six_value_list[2][i]),
        "阴攻": str(six_value_list[3][i]),
        "阴防": str(six_value_list[4][i]),
        "速度": str(six_value_list[5][i]),
        "属性每级强化": correction1_type_list[i] + '+' + str(correction1_value_diff_list[i]) + ' ' + correction2_type_list[
            i] + '+' + str(
            correction2_value_diff_list[i]),
        "获取途径": picture_source[str(id_list[i])] if str(id_list[i]) in picture_source else '国服暂无',
        "特性1": characteristic_list[i],
        "特性2": characteristic_max_list[i],
        "特性1简缩": '',
        "特性2简缩": '',
        "解说1": flavor_text1_list[i].replace('<ret>', '<br>'),
        "解说2": flavor_text2_list[i].replace('<ret>', '<br>'),
        "解说3": flavor_text3_list[i].replace('<ret>', '<br>'),
        "解说4": flavor_text4_list[i].replace('<ret>', '<br>'),
        "解说5": flavor_text5_list[i].replace('<ret>', '<br>'),
    }
    pictures_json[name_list[i]] = picture_dict
with open(target_path + 'pictures_wikidata.json', 'w', encoding='utf-8') as f:
    json_string = json.dumps(pictures_json, ensure_ascii=False, separators=(',\n', ': '))
    f.write(json_string)
    # {{绘卷图鉴
    # |名称=__名称__
    # |编号=__编号__
    # |类型=__类型__
    # |稀有度=__稀有度__
    # |画师=__画师__
    # |buff类型=__buff类型__
    # |debuff类型=__debuff类型__
    # |作用范围=__作用范围__
    # |弹种=__弹种__
    # |特性属性=__特性属性__
    # |其他效果=__其他效果__
    # |角色式限定=__角色式限定__
    # |查询属性=__查询属性__
    # |属性1=__属性1__
    # |属性2=__属性2__
    # |0级属性1=__0级属性1__
    # |0级属性2=__0级属性2__
    # |10级属性1=__10级属性1__
    # |10级属性2=__10级属性2__
    # |体力=__体力__
    # |阳攻=__阳攻__
    # |阳防=__阳防__
    # |阴攻=__阴攻__
    # |阴防=__阴防__
    # |速度=__速度__
    # |属性每级强化=__属性每级强化__
    # |获取途径=__获取途径__
    #
    # |特性1=__特性1__
    # |特性2=__特性2__
    # |特性1简缩=__特性1简缩__
    # |特性2简缩=__特性2简缩__
    #
    # |解说1=__解说1__
    # |解说2=__解说2__
    # |解说3=__解说3__
    # |解说4=__解说4__
    # |解说5=__解说5__
    #
    # }}
