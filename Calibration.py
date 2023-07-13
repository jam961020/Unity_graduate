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


count = 0

teststring = "hi_there?"
temp = ""
receivedData = ""

def exit_handler():
    exitmsg = "bye bye!"
    test2(exitmsg)
    sock.close()
    


def warpping(img): #원근변환 함수

    
    # 이미지의 높이, 너비 픽셀 좌표
    h = img.shape[0]
    w = img.shape[1]
    
    

    #대입할 이미지 중심 설정
    shift_w = w/2
    shift_h = 0

    # 원본 이미지 좌표 [x,y] (좌상 좌하 우상 우하)
    #화각 87도 기준
    ori_coordinate = np.float32([[50, 0], [135, 172], [590, 0], [505, 172]])

    #대입할 이미지 좌표 (좌상 좌하 우상 우하) 한칸이 15
    warped_coordinate = np.float32([[shift_w-60, shift_h], [shift_w-60, shift_h+40], [shift_w+60, shift_h], [shift_w+60, shift_h+40]])

    #3X3 변환 행렬 생성
    Matrix = cv2.getPerspectiveTransform(ori_coordinate, warped_coordinate)

    #원근 변환
    warped_img = cv2.warpPerspective(img, Matrix, (w, h))
    return warped_img


atexit.register(exit_handler)



host, port = "127.0.0.1", 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host,port))

sock.setblocking(True)

sock.sendall(teststring.encode("UTF-8"))

while True:
    receivedData = sock.recv(1024).decode("UTF-8")
    #test2(int(receivedData[-5]))
    if receivedData != "" and temp!= receivedData:

        #time.sleep(0.1)

        receiveImgN = cv2.imread(receivedData + "N.png")
        receiveImgS = cv2.imread(receivedData + "S.png")
        receiveImgE = cv2.imread(receivedData + "E.png")
        receiveImgW = cv2.imread(receivedData + "W.png")

        #if(receiveImgE == None): test2("E")
        #if(receiveImgN == None): test2("N")
        #if(receiveImgS == None): test2("S")
        #if(receiveImgW == None): test2("W")

        warp_N = warpping(receiveImgN)
        warp_W = warpping(receiveImgW)
        warp_S = warpping(receiveImgS)
        warp_E = warpping(receiveImgE)

        #이미지 방향에 맞게 회전시키기
        warp_W = imutils.rotate_bound(warp_W, 270)
        warp_N = imutils.rotate_bound(warp_N, 180)
        warp_E = imutils.rotate_bound(warp_E, 90)

        h1, w1, c1 = warp_W.shape
        
        #상하 이미지의 높이, 너비
        #480 640
        h2, w2, c2 = warp_N.shape

        #이격 19
        #검정색 배경 이미지 생성
        inn = 19
        b_w = 960+inn*2
        b_h = 960+inn*2
        background = np.zeros((b_w, b_h, 3), dtype=np.uint8)*255

        #배경 이미지에 좌우 이미지 합성
        #이미지 크기 640*480
        #480-320 : 480+320
        background[160+inn:800+inn, 0:w1] = warp_E
        background[160+inn:800+inn, w1+inn*2:b_w] = warp_W

        #or연산을 이용하여 배경이미지에 상하 이미지 합성
        roiN = background[0:h2, 160+inn:800+inn]
        roiS = background[h2+inn*2:b_h, 160+inn:800+inn]

        bit_n = cv2.bitwise_or(roiN, warp_N)
        bit_s = cv2.bitwise_or(roiS, warp_S)

        background[0:h2, 160+inn:800+inn] = bit_n
        background[h2+inn*2:b_h, 160+inn:800+inn] = bit_s

        #검정부분 잘라내기
        background = background[200:800, 200:800]

        sendstring = "C:/Users/user/UnityGraduate/PythonStream/test" + str(count) +".png"

        cv2.imwrite(sendstring,background)
        sock.sendall(sendstring.encode("UTF-8"))
        temp = receivedData
        count = count + 1
    else :
        sock.sendall("same!!!".encode("UTF-8"))
