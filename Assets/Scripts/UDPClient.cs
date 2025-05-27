using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPClient : MonoBehaviour
{
    // 나 자신
    public static UDPClient instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameObject.SetActive(false);
        }
    }

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

        // 서버에서 메시지 받을 함수 등록
        udpClient.BeginReceive(ReceiveData, null);

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

    void ReceiveData(IAsyncResult result)
    {
        // result 의 값을 byte 배열로 받아오자.
        byte[] receiveBytes = udpClient.EndReceive(result, ref endPoint);
        // byte 배열을  string 값으로 변환
        string receiveMessage = Encoding.UTF8.GetString(receiveBytes);

        print("서버에서 옴 : " + receiveMessage);
        GameManager.instance.OnOff(receiveMessage);

        // 서버에서 메시지 받을 함수 등록
        udpClient.BeginReceive(ReceiveData, null);
    }

    private void OnDestroy()
    {
        // 클라이언트 종료
        if(udpClient != null)
        {
            udpClient.Close();

            print("클라이언트 종료");
        }
    }
}
