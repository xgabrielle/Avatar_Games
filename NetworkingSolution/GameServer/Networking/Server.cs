using System.Net;
using System.Net.Sockets;

namespace GameServer.Networking;

public class Server
{
    private TcpListener _listener;
    private readonly List<Client> _clients = new();
    private int port = 3030;

    public void OnStart()
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        Console.WriteLine($"Server start on Port {port}");

        while (true)
        {
            TcpClient tcpClient = _listener.AcceptTcpClient();
            Console.WriteLine("Client is connected");

            string role = null!; 
            if (_clients.Count == 0) role = "White Markers";
            if (_clients.Count == 1) role = "Dark Markers";
            
            Console.WriteLine($"Assigned {role} to client {tcpClient.Client.RemoteEndPoint}");
            
            Client clientHandler = new Client(tcpClient, this, role);
            
            if (_clients.Count < 2) _clients.Add(clientHandler);
            
            new Thread(clientHandler.Handle).Start();
        }
    }

    public void Broadcast(string message, Client? excludeClient = null)
    {
        foreach (var client in _clients)
        {
            if (client != excludeClient)
                client.Send(message);
        }
    }

    public void RemoveClient(Client client)
    {
        _clients.Remove(client);
        Console.WriteLine("Client disconnected");
    }
}