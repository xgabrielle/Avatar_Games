using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using NetworkProtocolLib;

public class RoleManager
{
    public static string Role;
}
public class NetworkClient : MonoBehaviour
{
    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;
    private CheckersMove _lastMove;
    public static NetworkClient Client { get; private set; }

    void Start()
    {
        if (Client == null) Client = this;
        else Destroy(Client);
    }

    public async void StartMultiplayerConnection()
    {
        try
        {
            ConnectToServer();
            await Listen();
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection error: {e.Message}");
            throw;
        }
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
            
            if (roleMessage!.StartsWith("Player:"))
            {
                string role = roleMessage.Split(":")[1];
                RoleManager.Role = role;
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

    private async Task Listen()
    {
        while (true)
        {
            if (_reader == null)
            {
                Debug.LogWarning("[Client] Reader is null.");
                await Task.Delay(100);
                continue;
            }

            string message = await _reader.ReadLineAsync();
            if (string.IsNullOrEmpty(message))
            {
                await Task.Delay(100);
                continue;
            }

            Debug.Log($"[NetworkClient] Received: {message}");

            var (type, data) = NetworkProtocol.ParseMessage(message);

            switch (type)
            {
                case "MOVE":
                    HandleMove(data);
                    break;
                case "TURN":
                    TurnManager.instance.SwitchTurn();
                    break;
            }
        }
    }
    void HandleMove(string moveData)
    {
        //Debug.Log($"[NetworkClient] Received MOVE from: {moveData}");
        Debug.Log($"[NetworkClient] I am seeing a move from: {moveData}");

        string[] parts = moveData.Split("-");

        Vector3Int from = ParseCoordinates(parts[0]);
        Vector3Int to = ParseCoordinates(parts[1]);

        _lastMove = new CheckersMove();
        _lastMove.from = from;
        _lastMove.to = to;
        TurnManager.instance.SwitchTurn();
    }

    Vector3Int ParseCoordinates(string coordinates)
    {
        string[] xz = coordinates.Split(",");
        return new Vector3Int(int.Parse(xz[0]), 0,int.Parse(xz[1]));
    }

    public void SendMove(Vector3 from, Vector3 to)
    {
        string moveData = $"{from.x},{from.z} - {to.x},{to.z}";
        string message = NetworkProtocol.CreateMessage("MOVE",moveData);
        
        Debug.Log($"[NetworkClient] Sending move message: {message}");
        
        _writer.WriteLine(message);
        _writer.Flush();
    }
}
