import re
import sys
file_name = sys.argv[1]
f = open(file_name)
fo = open(file_name.split('.')[0] + "-output.csv", 'w')
for line in f:
	fields = line.strip().split()
	if fields[0] == 'id':
		assert fields[-1] == 'is_show'
	new_line = re.sub(r',(is_show|0|1)$', '', line.strip())
	fo.write(new_line + "\n")
fo.close()
# compare: git diff --no-index