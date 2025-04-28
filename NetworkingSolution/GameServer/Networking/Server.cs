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

    public void OnStart()
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        //Console.WriteLine($"Server start on Port {port}");

        while (true)
        {
            TcpClient tcpClient = _listener.AcceptTcpClient();
            //Console.WriteLine("Client is connected");
            Console.WriteLine($"[{DateTime.Now}] [Server] Client connected: {tcpClient.Client.RemoteEndPoint}");

            string role = null!; 
            if (_clients.Count == 0) role = "White Markers";
            if (_clients.Count == 1) role = "Dark Markers";
            
            
            Client clientHandler = new Client(tcpClient, this, role);
            if (_clients.Count < 2) _clients.Add(clientHandler);
            
            clientHandler.Send($"Player:{role}");
            Console.WriteLine($"Assigned {role} to client {tcpClient.Client.RemoteEndPoint}");
            
            
            new Thread(clientHandler.Handle).Start();
        }
    }
    
    public void BroadcastToClient(string message, Client? excludeClient)
    {
        //Console.WriteLine($"[Server] Broadcasting the message: {message}");
        foreach (var client in _clients)
        {
            //Console.WriteLine($"[Server] Checking client: {client.GetHashCode()}");
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
            //Console.WriteLine($"[Server] Broadcasting to all: {message}");
            client.Send(message);
        }
    }
    
    public void HandlePlayerTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % _clients.Count;
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
    
}