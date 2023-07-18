def test():
    from tkinter import messagebox
    messagebox.showinfo("알림창","딸랑딸랑")

def test2(msg):
    from tkinter import messagebox
    messagebox.showinfo("알림창",msg)

#test해보고 싶으면 주석 해제
#test()

import socket
import cv2
import atexit
import imutils as imutils
import numpy as np
import time
import matplotlib.pyplot as plt

count = 0

teststring = "hi_there?"
temp = ""
receivedData = r"C:\Users\user\UnityGraduate\UnityStreamtest\87"

def exit_handler():
    exitmsg = "bye bye!"
    test2(exitmsg)
    
def warpping2(img): #원근변환 함수


    # 이미지의 높이, 너비 픽셀 좌표
    h = img.shape[0]
    w = img.shape[1]

    # 대입할 이미지 중심 설정
    shift_w = w / 2
    shift_h = 0

    # 원본 이미지 좌표 [x,y] (좌상 좌하 우상 우하)
    # 화각 87도 기준
    ori_coordinate = np.float32([[50, 0], [21, 411], [590, 0], [611, 404]])

    # 대입할 이미지 좌표 (좌상 좌하 우상 우하) 한칸이 15
    warped_coordinate = np.float32([[shift_w - 59, shift_h], [shift_w - 278, shift_h + 300], [shift_w + 59, shift_h], [shift_w + 255, shift_h + 278]])

    # 3X3 변환 행렬 생성
    Matrix = cv2.getPerspectiveTransform(ori_coordinate, warped_coordinate)

    # 원근 변환
    warped_img = cv2.warpPerspective(img, Matrix, (w, h))
    return warped_img


def warpping(img): #원근변환 함수

    
    # 이미지의 높이, 너비 픽셀 좌표
    h = img.shape[0]
    w = img.shape[1]
    
    

    #대입할 이미지 중심 설정
    shift_w = w/2
    shift_h = 0

    # 원본 이미지 좌표 [x,y] (좌상 좌하 우상 우하)
    #화각 87도 기준
    ori_coordinate = np.float32([[51, 0], [135, 172], [588, 0], [505, 172]])

    #대입할 이미지 좌표 (좌상 좌하 우상 우하) 한칸이 15
    warped_coordinate = np.float32([[shift_w-60, shift_h], [shift_w-60, shift_h+40], [shift_w+60, shift_h], [shift_w+60, shift_h+40]])

    #3X3 변환 행렬 생성
    Matrix = cv2.getPerspectiveTransform(ori_coordinate, warped_coordinate)

    #원근 변환
    warped_img = cv2.warpPerspective(img, Matrix, (w, h))
    return warped_img


atexit.register(exit_handler)



#host, port = "127.0.0.1", 25001
#sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
#sock.connect((host,port))

#sock.setblocking(True)

#sock.sendall(teststring.encode("UTF-8"))


receiveImgN = cv2.imread(receivedData + "N.png")
receiveImgS = cv2.imread(receivedData + "S.png")
receiveImgE = cv2.imread(receivedData + "E.png")
receiveImgW = cv2.imread(receivedData + "W.png")

warp_N = warpping2(receiveImgN)
warp_W = warpping2(receiveImgW)
warp_S = warpping2(receiveImgS)
warp_E = warpping2(receiveImgE)

#이미지 방향에 맞게 회전시키기
warp_W = imutils.rotate_bound(warp_W, 270)
warp_N = imutils.rotate_bound(warp_N, 180)
warp_E = imutils.rotate_bound(warp_E, 90)

cv2.imshow('E',warp_E)
cv2.imshow('W',warp_W)
cv2.imshow('S',warp_S)
cv2.imshow('N',warp_N)
 # Figure와 Axes 생성
fig, axs = plt.subplots(2, 2)

# warp_W 이미지 표시
axs[0, 0].imshow(warp_W)
axs[0, 0].set_title('W')

# warp_N 이미지 표시
axs[0, 1].imshow(warp_N)
axs[0, 1].set_title('N')

# warp_E 이미지 표시
axs[1, 0].imshow(warp_E)
axs[1, 0].set_title('E')

# warp_S 이미지 표시
axs[1, 1].imshow(warp_S)
axs[1, 1].set_title('S')

# 각 이미지의 타이틀과 눈금 제거
for ax in axs.flat:
    ax.axis('off')

# 그림 출력
#plt.show() 


h1, w1, c1 = warp_W.shape
        
#상하 이미지의 높이, 너비
#480 640
h2, w2, c2 = warp_N.shape

#이격 19
#검정색 배경 이미지 생성
inn = 14
b_w = 960+inn*2
b_h = 960+inn*2
background = np.zeros((b_w, b_h, 3), dtype=np.uint8)*255

#배경 이미지에 좌우 이미지 합성
#이미지 크기 640*480
#480-320 : 480+320
background[160+inn:800+inn, 0:w1] = warp_E
background[160+inn:800+inn, w1+inn*2:b_w] = warp_W

cv2.imshow('we',background)

#or연산을 이용하여 배경이미지에 상하 이미지 합성
roiN = background[0:h2, 160+inn:800+inn]
roiS = background[h2+inn*2:b_h, 160+inn:800+inn]

bit_n = cv2.bitwise_or(roiN, warp_N)
bit_s = cv2.bitwise_or(roiS, warp_S)

background[0:h2, 160+inn:800+inn] = bit_n
background[h2+inn*2:b_h, 160+inn:800+inn] = bit_s

#검정부분 잘라내기
background = background[200:800, 200:800]

# testim = [warp_E,warp_W]

# stitcher = cv2.Stitcher_create()
# status, testres = stitcher.stitch(testim)

# if status == cv2.Stitcher_OK:
#     test2("Stitching Success!")
#     cv2.imshow("test", testres)
# else:
#     test2("Stitching Failed!")

# cv2.imshow("test",testres)

sendstring = "C:/Users/user/UnityGraduate/PythonStreamtest/test" + str(10)

cv2.imwrite(sendstring + "N.png",warp_N)
cv2.imwrite(sendstring + "E.png",warp_E)
cv2.imwrite(sendstring + "S.png",warp_S)
cv2.imwrite(sendstring + "W.png",warp_W)

cv2.imwrite(sendstring + ".png",background)