using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;
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

    // 서버에 보내진 데이터를 쌓아놓는 변수
    ConcurrentQueue<string> allMessage = new ConcurrentQueue<string>();

    // NetView 오브젝트 가지고 있는 Dictionary
    public Dictionary<long, NetView> allNetView = new Dictionary<long, NetView>();
    // 마지막 NetView 의 id
    long lastNetid = 1000;

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
        while(allMessage.TryDequeue(out string msg))
        {
            // msg -> JObject 로 변환
            JObject jObject = JObject.Parse(msg);
            if (jObject.ContainsKey("net_id") == false) continue;

            long net_id = jObject["net_id"].ToObject<long>();
            // allNetView 의 net_id 에 해당되는 오브젝트의 OnMessage 함수 실행
            allNetView[net_id].OnMessage(msg);
        }
    }

    public long AddNetView(NetView view)
    {
        long id = view.netId;
        // 만약에 netId 가 0이면
        if(id == 0)
        {
            // 마지막 NetView 의 id 증가
            lastNetid++;
            id = lastNetid;
        }

        // allNetView 에 추가 (Key : lastNetid, value : view)
        allNetView[id] = view;

        return id;
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
        
        // receiveMessage 큐에 쌓자
        allMessage.Enqueue(receiveMessage);

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
