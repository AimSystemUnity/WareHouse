using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class UDPClient : MonoBehaviour
{
    public static UDPClient instance;

    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;

    public string ip = "127.0.0.1";
    public int port = 7777;

    List<string> allMessage = new List<string>();
    public Action<string> onMessage;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartUDPClient();
    }

    private void Update()
    {

        if(allMessage.Count > 0)
        {
            if (onMessage != null)
            {
                List<string> msgList = allMessage;

                for(int i = 0; i < msgList.Count; i++)
                {
                    string msg = msgList[0];
                    allMessage.RemoveAt(0);
                    onMessage(msg);
                }
            }
        }
    }

    public void StartUDPClient()
    {
        udpClient = new UdpClient();
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        // Start receiving data asynchronously
        udpClient.BeginReceive(ReceiveData, null);

        // Send a message to the server
        SendData("Hello, server!");
    }

    private void ReceiveData(IAsyncResult result)
    {
        byte[] receivedBytes = udpClient.EndReceive(result, ref remoteEndPoint);
        string receivedMessage = System.Text.Encoding.UTF8.GetString(receivedBytes);

        Debug.Log("Received from server: " + receivedMessage);

        allMessage.Add(receivedMessage);

        // Continue receiving data asynchronously
        udpClient.BeginReceive(ReceiveData, null);

        
    }

    private void SendData(string message)
    {
        byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(message);

        // Send the message to the server
        udpClient.Send(sendBytes, sendBytes.Length, remoteEndPoint);

        Debug.Log("Sent to server: " + message);
    }

    public void SendData(byte[] sendBytes)
    {
        // Send the message to the server
        udpClient.Send(sendBytes, sendBytes.Length, remoteEndPoint);

        Debug.Log("sendBytes : " + sendBytes.Length);
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
        Debug.Log("UDP Client stopped.");
    }
}
