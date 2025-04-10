using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class NetworkClient : MonoBehaviour
{
    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;
    void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        try
        {
            _tcpClient = new TcpClient("127.0.0.1", 3030);
            _stream = _tcpClient.GetStream();
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream);
            string roleMessage = _reader.ReadLine();
            Debug.Log("Connect to Server Unity");
            if (roleMessage.StartsWith("Player:"))
            {
                string role = roleMessage.Split(":")[1];
                Debug.Log($"You are role: {role}");
            }
            
            _writer.WriteLine("Hello from unity client");
            _writer.Flush();
            
            
            Debug.Log($"Server response: {roleMessage}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection error: {e.Message}");
            throw;
        }

    }
}
