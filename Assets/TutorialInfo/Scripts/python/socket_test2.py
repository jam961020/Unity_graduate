import socket
import time

host, port = "127.0.0.1", 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host, port))

startPos = [0,0,0]
while True:
    #time.sleep(0.5)
    startPos[0] +=1
    posString = ','.join(map(str, startPos))
    print(posString)

    sock.sendall(posString.encode("UTF-8"))
    receivedData = sock.recv(1024).decode("UTF-8")
    print(receivedData)
    