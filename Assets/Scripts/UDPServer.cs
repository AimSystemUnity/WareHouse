using System;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Collections.Generic;

public enum ENetType
{
    NET_CONVEYOR_IS_ON,
    NET_CREATE_MY_OBJECT,
    NET_POSITION
}

public class UDPServer : MonoBehaviour
{
    public static UDPServer instance;
    private UdpClient udpServer;
    List<IPEndPoint> listEndPoint = new List<IPEndPoint>();


    public int serverPort = 7777;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        // Example: Start the UDP server on port 5555
        StartUDPServer(serverPort);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendData("server!");
        }
    }

    private void StartUDPServer(int port)
    {
        udpServer = new UdpClient(port);

        Debug.Log("Server started. Waiting for messages...");

        // Start receiving data asynchronously
        udpServer.BeginReceive(ReceiveData, null);
    }


    private void ReceiveData(IAsyncResult result)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);

        //byte[] receivedBytes = udpServer.EndReceive(result, ref remoteEndPoint);
        byte[] receivedBytes = udpServer.EndReceive(result, ref endPoint);
        string receivedMessage = System.Text.Encoding.UTF8.GetString(receivedBytes);
        Debug.Log("Received from client: " + receivedMessage);

        if (!listEndPoint.Contains(endPoint))
        {
            listEndPoint.Add(endPoint);
        }

        // Process the received data
        // Continue receiving data asynchronously
        udpServer.BeginReceive(ReceiveData, null);

    }

    public void SendData(string message)
    {
        byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(message);

        // Send the message to the client
        for (int i = 0; i < listEndPoint.Count; i++)
        {
            udpServer.Send(sendBytes, sendBytes.Length, listEndPoint[i]);
            Debug.Log("Sent to client: " + message);
        }
    }


    void OnApplicationQuit()
    {
        udpServer.Close();
        Debug.Log("UDP Server stopped.");
    }
}