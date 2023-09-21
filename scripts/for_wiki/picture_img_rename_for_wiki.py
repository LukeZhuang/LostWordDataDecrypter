import os
import shutil
import csv

picture_name = {}

with open("../../decrypt_output/PictureTable.csv") as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader:
        picture_name[row["id"]] = row["name"]

IMG_PATH = "./extract_output/04f2dbc8936a12ff5d3b5e1adb8553f1/ExportedProject/Assets/east/pictures"
OUTPUT_PATH = "./output"

for pic_id in os.listdir(IMG_PATH):
    prefix = os.path.join(IMG_PATH, pic_id)
    pic_name = picture_name[pic_id]
    srcfile = os.path.join(prefix, "Efuda.png")
    if os.path.exists(srcfile):
        destfile = os.path.join(OUTPUT_PATH, "绘卷 " + pic_id + pic_name + ".png")
        shutil.copy(srcfile, destfile)
    else:
        print(pic_id, pic_name, "Efuda.png does not exists")

    srcfile = os.path.join(prefix, "ThumbLarge.png")
    if os.path.exists(srcfile):
        destfile = os.path.join(OUTPUT_PATH, "绘卷图鉴 " + pic_id + pic_name + ".png")
        shutil.copy(srcfile, destfile)
    else:
        print(pic_id, pic_name, "ThumbLarge.png does not exists")

    srcfile = os.path.join(prefix, "ThumbSquare.png")
    if os.path.exists(srcfile):
        destfile = os.path.join(OUTPUT_PATH, "绘卷图标 " + pic_id + pic_name + ".png")
        shutil.copy(srcfile, destfile)
    else:
        print(pic_id, pic_name, "ThumbSquare.png does not exists")
