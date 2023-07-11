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
import time
import threading
import os


count = 0

teststring = "hi_there?"
temp = ""
receivedData = ""


def exit_handler():
    exitmsg = "bye bye!"
    test2(exitmsg)
    sock.close()
    

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
        count = count + 1
        receiveImg = cv2.imread(receivedData, cv2.IMREAD_GRAYSCALE)
        sendstring = "C:/Users/user/UnityGraduate/PythonStream/test" + str(count) +".png"
        cv2.imwrite(sendstring,receiveImg)
        sock.sendall(sendstring.encode("UTF-8"))
        temp = receivedData
    else :
        sock.sendall("same!!!".encode("UTF-8"))
