using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPServer : MonoBehaviour
{
    // 서버 객체 
    UdpClient udpServer;

    // port 번호
    int port = 7777;

    void Start()
    {
        // 서버 객체 생성
        udpServer = new UdpClient(port);

        print("서버 시작!!!");

        // 클라이언트에서 메시지 받을 함수 등록
        udpServer.BeginReceive(ReceiveData, null);
    }

    void Update()
    {
        
    }

    void ReceiveData(IAsyncResult result)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
        // result 의 값을 byte 배열로 받아오자.
        byte[] receiveBytes = udpServer.EndReceive(result, ref endPoint);
        // byte 배열을  string 값으로 변환
        string receiveMessage = Encoding.UTF8.GetString(receiveBytes);

        print(receiveMessage);

        // 클라이언트에서 메시지 받을 함수 등록
        udpServer.BeginReceive(ReceiveData, null);
    }

    private void OnDestroy()
    {
        // 서버 종료
        udpServer.Close();

        print("서버 종료");
    }
}
