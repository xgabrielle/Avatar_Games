using System.Net.Sockets;
using NetworkProtocolLib;
namespace GameServer.Networking;

public class Client
{
    private readonly TcpClient _client;
    private readonly Server _server;
    private StreamReader _reader;
    private StreamWriter _writer;

    public Client (TcpClient client, Server server)
    {
        _client = client;
        _server = server;
    }

    public void Handle()
    {
        try
        {
            using NetworkStream stream = _client.GetStream();
            
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };
            _writer.WriteLine("Hello from Console client");
            while (true)
            { 
                string message = _reader.ReadLine();
                if (message == null) break;
                ProcessMessage(message);
                Console.WriteLine($"Received: {message}");
                _server?.BroadcastToClient(message, this);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client error: {ex.Message}");
        }
        finally
        {
            _client.Close();
            _server?.RemoveClient(this);
        }
    }

    void ProcessMessage(string message)
    {
        var (type, data) = NetworkProtocol.ParseMessage(message);

        switch (type)
        {
            case "MOVE":
                HandleMove(data);
                break;
            case "CHAT":
                HandleChat(data);
                break;
            case "TURN":
                _server.HandlePlayerTurn();
                break;
            
        }
    }

    void HandleMove(string moveData)
    {
        Console.WriteLine($"Move by client: {moveData}");
        _server.BroadcastToClient($"Move: {moveData}", this);
        _server.Broadcast($"Turn: {_server.GetPlayerTurn()}");
    }

    

    void HandleChat(string message)
    {
        
    }
    
    public void Send(string message)
    {
        try
        {
            _writer.WriteLine(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Send error: {ex.Message}");
        }
    }
}