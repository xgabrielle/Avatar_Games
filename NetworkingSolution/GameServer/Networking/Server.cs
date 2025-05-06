using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace GameServer.Networking;

public class Server
{
    private TcpListener _listener;
    private readonly List<Client> _clients = new();
    int currentTurnIndex = 0;
    private int port = 3030;
    private bool gameStarting = true;

    public void OnStart()
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();

        while (true)
        {
            TcpClient tcpClient = _listener.AcceptTcpClient();
            
            Console.WriteLine($"[{DateTime.Now}] [Server] Client connected: {tcpClient.Client.RemoteEndPoint}");

            string role = null!; 
            if (_clients.Count == 0) role = "White Markers";
            if (_clients.Count == 1) role = "Dark Markers";
            
            
            Client clientHandler = new Client(tcpClient, this, role);
            if (_clients.Count < 2) _clients.Add(clientHandler);
            
            clientHandler.Send($"Player:{role}");
            Console.WriteLine($"Assigned {role} to client {tcpClient.Client.RemoteEndPoint}");

            new Thread(clientHandler.Handle).Start();
            
            if (_clients.Count == 2)
            {
                Console.WriteLine($"[{DateTime.Now}] [Server] Game is ready to start with 2 players");
                Broadcast("START");
                
                string initialTurnMessage = NetworkProtocolLib.NetworkProtocol.CreateMessage("TURN", _clients[currentTurnIndex].GetRole());
                Broadcast(initialTurnMessage);
                
                //Thread.Sleep(50); // Give time for initial messages
                gameStarting = false;
            }
        }
    }
    
    public void BroadcastToClient(string message, Client? excludeClient)
    {
        foreach (var client in _clients)
        {
            if (client != excludeClient)
            {
                client.Send(message);
            }
        }
    }

    public void Broadcast(string message)
    {
        foreach (var client in _clients)
        {
            client.Send(message);
        }
    }
    
    public void HandlePlayerTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % _clients.Count;
        Console.WriteLine($"[{DateTime.Now}] [Server] Turn changed to player: {_clients[currentTurnIndex].GetRole()}");
    }
    public Client GetPlayerTurn()
    {
        return _clients[currentTurnIndex];
    }
    
    public void RemoveClient(Client client)
    {
        _clients.Remove(client);
        Console.WriteLine("Client disconnected");
    }

    public bool IsGameStarting()
    {
        return gameStarting;
    }
}