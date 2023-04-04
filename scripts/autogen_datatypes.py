import re

INDENT = "    "
VER = "ver2"


def get_class_content(class_line_num, lines):
	result = {}
	for class_name in class_line_num:
		class_result = []
		line_num = class_line_num[class_name]
		assert lines[line_num + 1] == '{'
		if lines[line_num - 1] != '// Namespace:':
			assert lines[line_num - 1] == '[Serializable]'
			class_result.append('[Serializable]')
		class_result.append(lines[line_num])
		class_result.append('{')
		if lines[line_num + 2] == '// Fields':
			class_result.append(INDENT + '// Fields')
			if lines[line_num + 3].startswith('[MessagePackMember'):
				i = line_num + 4
				while lines[i].startswith('public'):
					assert lines[i - 1].startswith('[MessagePackMember')
					class_result.append(INDENT + lines[i - 1])
					class_result.append(INDENT + lines[i])
					i += 2
			else:
				i = line_num + 3
				while lines[i].startswith('public'):
					class_result.append(INDENT + lines[i])
					i += 1
		class_result.append('}')
		result[class_name] = class_result
	return result


f = open('../dump.cs')
lines = []
master_line_num = {}
table_line_num = {}
record_line_num = {}
for (idx, line) in enumerate(f):
	l = line.strip()
	lines.append(l)
	master_gp = re.findall("class ([a-zA-Z]+)Master", l)
	table_gp = re.findall("class ([a-zA-Z]+)Table : MasterTable", l)
	record_gp = re.findall("class ([a-zA-Z]+)Record : MasterRecord", l)
	if len(master_gp) > 0:
		master_line_num[master_gp[0]] = idx
	if len(table_gp) > 0:
		table_line_num[table_gp[0]] = idx
	if len(record_gp) > 0:
		record_line_num[record_gp[0]] = idx

# print(len(master_line_num.items()))
# print(len(table_line_num.items()))
# print(len(record_line_num.items()))

masters = get_class_content(master_line_num, lines)
tables = get_class_content(table_line_num, lines)
records = get_class_content(record_line_num, lines)


for k in record_line_num:
	assert k in table_line_num
	assert k in master_line_num

for k in table_line_num:
	assert k in master_line_num

for k in master_line_num:
	fo = open(k + "Master.cs", "w")
	fo.write("using MsgPack.Serialization;\n")
	fo.write("using System;\n")
	fo.write("\n")
	fo.write("namespace LostWordDataDecrypter.DataTypes.DataTypes_" + VER + "\n")
	fo.write("{\n")
	if k in records:
		for l in records[k]:
			fo.write(INDENT + l + "\n")
	fo.write("\n")
	if k in tables:
		for l in tables[k]:
			fo.write(INDENT + l + "\n")
	fo.write("\n")
	if k in masters:
		for l in masters[k]:
			fo.write(INDENT + l + "\n")
	fo.write("\n")
	fo.write("}\n")
	fo.close()
