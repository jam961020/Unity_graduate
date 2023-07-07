using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SocketManager : MonoBehaviour
{
    Thread mThread;
    public string connectionIp = "127.0.0.1";
    public int connectionPort = 25001;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    string streamImagePath;
    bool running;
    bool IsRecevingPath;
    StreamingManager streamingManager;

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIp);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
        client = listener.AcceptTcpClient();
        running = true;
        while(running)
        {
            IsRecevingPath = SendAndReceiveData();
        }
        listener.Stop();
    }

    bool SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();

        byte[] myWriteBuffer = Encoding.ASCII.GetBytes(streamingManager.unityPath());
        nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);

        byte[] buffer = new byte[client.ReceiveBufferSize];
        // 호스트로 부터 받은 데이터라는데 잘 모르겠다.
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        if (dataReceived != null)
        {
            streamImagePath = dataReceived;
            Debug.Log("receving");
            return true;
        }
        else return false;
    }
    public bool RecevingPath()
    {
        return IsRecevingPath;
    }
    // Start is called before the first frame update
    void Start()
    {
        streamingManager = GetComponent<StreamingManager>();
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsRecevingPath) Debug.Log(streamImagePath);
    }
}
