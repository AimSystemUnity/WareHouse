using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public enum ENetType
{
    NET_CONVEYOR_IS_ON,
    NET_CREATE_MY_OBJECT,
    NET_ADD_OBJECT,
    NET_BOT_TRANSFORM,
    NET_SEND_TRIGGER
}

public class UDPServer : MonoBehaviour
{
    // 나 자신
    public static UDPServer instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameObject.SetActive(false);
        }
    }


    // 서버 객체 
    UdpClient udpServer;

    // port 번호
    int port = 7777;

    // 클라이언트들의 IPEndPoint
    List<IPEndPoint> listEndPoint = new List<IPEndPoint>();

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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendData("1 번키 누름");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SendData("2 번키 누름");
        }
    }

    public void SendData(string message)
    {
        // string 을 byte 배열로
        byte[] sendBytes = Encoding.UTF8.GetBytes(message);

        // byte 배열을 클라이언트에게 알려주자.
        for(int i = 0; i < listEndPoint.Count; i++)
        {
            udpServer.Send(sendBytes, sendBytes.Length, listEndPoint[i]);
        }
    }

    void ReceiveData(IAsyncResult result)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
        // result 의 값을 byte 배열로 받아오자.
        byte[] receiveBytes = udpServer.EndReceive(result, ref endPoint);
        // byte 배열을  string 값으로 변환
        string receiveMessage = Encoding.UTF8.GetString(receiveBytes);

        print(receiveMessage);

        // 접속된 클라이언트의 endPoint 저장
        if(listEndPoint.Contains(endPoint) == false)
        {
            listEndPoint.Add(endPoint);
        }

        // 클라이언트에서 메시지 받을 함수 등록
        udpServer.BeginReceive(ReceiveData, null);
    }

    private void OnDestroy()
    {
        if(udpServer != null)
        {
            // 서버 종료
            udpServer.Close();

            print("서버 종료");
        }
    }
}
