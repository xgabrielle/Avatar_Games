using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using NetworkProtocolLib;
using dotenv.net;
public class RoleManager
{
    public static string Role;
}
public class NetworkClient : MonoBehaviour
{
    private string _ipAddress; 
    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;
    private CheckersMove _lastMove;
    public static NetworkClient Client { get; private set; }

    public static Action WaitingForPlayer;
    public static Action OnPlayerConnect;
    
    void Start()
    {
        DotEnv.Load();
        if (Client == null) Client = this;
        else Destroy(Client);
    }

    public async void StartMultiplayerConnection(string ip)
    {
        try
        {
            ConnectToServer(ip);
            await Task.Run(() => Listen());
        }
        catch (Exception e)
        {
            Debug.LogError($"[{DateTime.Now}] [Unity Client] Exception: {e.Message}");

            throw;
        }
    }

    private void ConnectToServer(string ip)
    {
        try
        {
            Debug.Log($"IP: {ip}");
            Debug.Log($"[{DateTime.Now}] [Unity Client] Attempting to connect to server...");
            _tcpClient = new TcpClient(ip, 3030);
            _stream = _tcpClient.GetStream();
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream);
            
            string roleMessage = _reader.ReadLine();
            Debug.Log($"[{DateTime.Now}] [Unity <- Server] Received handshake: {roleMessage}");
            

            
            if (roleMessage!.StartsWith("Player:"))
            {
                string role = roleMessage.Split(":")[1];
                RoleManager.Role = role;
                Debug.Log($"You are role: {role}");
                
                if (role == "White Markers")
                    WaitingForPlayer.Invoke();
                else if (role == "Dark Markers") 
                    OnPlayerConnect.Invoke();
                Debug.Log($"Sent role {role} to {_tcpClient.Client.RemoteEndPoint}");

            }
            _writer.WriteLine("Hello from unity client");
            _writer.Flush();
            
            
            Debug.Log($"Server response: {roleMessage}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[{DateTime.Now}] [Unity Client] Exception: {e.Message}");

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

            string message = (await _reader.ReadLineAsync())?.Trim();
            Debug.Log($"[{DateTime.Now}] [Unity <- Server] RAW RECEIVE: '{message}'");

            if (string.IsNullOrEmpty(message))
            {
                await Task.Delay(100);
                continue;
            }

            var (type, data) = NetworkProtocol.ParseMessage(message);
            Debug.Log($"[{DateTime.Now}] [Unity Client] Parsed: type={type}, data={data}");

            switch (type)
            {
                case "START":
                    Debug.Log("Received START message");
                    MainThreadDispatcher.Run(() =>
                    {
                        UIPersonality.instance.StartVSGame();
                        Debug.Log("First player wait sign removed");
                    });
                    break;
                case "MOVE":
                    HandleMove(data);
                    break;
                case "TURN":
                    HandleTurn();
                    break;
            }
        }
    }
    void HandleMove(string moveData)
    {
        
        Debug.Log($"[NetworkClient] I am seeing a move from: {moveData}");

        string[] parts = moveData.Split("-");

        Vector3Int from = ParseCoordinates(parts[0]);
        Vector3Int to = ParseCoordinates(parts[1]);

        _lastMove = new CheckersMove();
        _lastMove.from = new int[] { from.x, from.y, from.z };
        _lastMove.to = new int[] { to.x, to.y, to.z };
        MainThreadDispatcher.Run(() =>
        {
            GameObject pawn = GetPawn(from);
            if (pawn == null)
            {
                Debug.Log("No pawn to move");
                return;
            }
            Vector3 targetPos = new Vector3(to.x, 0.6f, to.z);
            Vector3 startPos = new Vector3(from.x, 0.6f, from.z);
            pawn.transform.position = targetPos;
            
            MarkersGenerator.instance.UpdatePawns(pawn, startPos, targetPos);
            TurnManager.instance.SwitchTurn();
        });
    }

    GameObject GetPawn(Vector3Int from)
    {
        Vector3 worldFrom = new Vector3(from.x, 0.6f, from.z);
        Collider[] hits = Physics.OverlapSphere(worldFrom, 0.2f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("WhiteMarker") || hit.CompareTag("DarkMarker"))
            {
                GameObject pawn = hit.gameObject;
                return pawn;
            }
        }
        return null;
    }

    void HandleTurn()
    {
        Debug.Log($"{TurnManager.instance.currentPlayer} turn to play");
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
        Debug.Log($"[{DateTime.Now}] [Unity -> Server] Sending move: {message}");
        
        
        _writer.WriteLine(message);
        _writer.Flush();
        Debug.Log($"[{DateTime.Now}] [Unity -> Server] Sent: Hello from unity client");
    }
    public void StartMultiplayer(string ip)
    {
        StartMultiplayerConnection(ip);
    }
    
    public void OnDisconnectClicked()
    {
        
    }

}
