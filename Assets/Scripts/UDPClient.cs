using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPClient : MonoBehaviour
{
    // 클라이언트 객체
    UdpClient udpClient;

    // endPoint
    IPEndPoint endPoint;

    // ip
    string ip = "127.0.0.1";
    // port
    int port = 7777;

    void Start()
    {
        // 클라이언트 객체 만들자.
        udpClient = new UdpClient();
        // endpoint 만들자.
        endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        // 서버 접속 메시지
        SendData("클라이언트 접속 시도");
    }

    void Update()
    {
        
    }

    void SendData(string message)
    {
        // string 을 byte 배열로 바꾸자
        byte[] sendBytes = Encoding.UTF8.GetBytes(message);
        // 서버에게 전송
        udpClient.Send(sendBytes, sendBytes.Length, endPoint);
    }

    private void OnDestroy()
    {
        // 클라이언트 종료
        udpClient.Close();

        print("클라이언트 종료");
    }
}
