length = 60
width = 80
image_name = 'DEMO_MAP.bmp'

import numpy as np
import cv2

def get_img(file_name):
  img=cv2.imread(file_name,cv2.IMREAD_GRAYSCALE)
  #cv2.waitKey(0)
  return (img.reshape(1,1,length,width))

aimg = get_img(image_name)

print(type(aimg))
print(aimg.shape)

for elem in aimg:
  print(elem)
  print(type(elem))
  print(elem[0,2,:])
  print(type(elem[0,2,:]))
  #print(int (elem))

f = open('opt.csv',mode='w')
str = ''
for j in range(0,length):
  for i in range(0,width-1):
    str = str + np.array2string(elem[0,j,i]) +','
  str = str + np.array2string(elem[0,j,width-1])
  print(str)
  f.write(str+'\n')
  str = ''
