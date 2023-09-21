from bs4 import BeautifulSoup

f = open('source.html')
fo = open('PictureSource.csv', 'w')
contents = f.read()
contents = contents.replace('\n', '')
soup = BeautifulSoup(contents, 'html')

table = soup.table
body = table.tbody

results = {}

for tr in body:
	tds = []
	for td in tr:
		tds.append(td)
	results[int(tds[2].text)] = tds[2].text + "," + tds[4].text + "," + tds[5].text

l = [p[1] for p in sorted(results.items(), key=lambda x:x[0])]
fo.write("id,name,source\n")
for a in l:
	fo.write(a + "\n")