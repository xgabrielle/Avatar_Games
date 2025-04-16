using System.Net.Sockets;
using NetworkProtocolLib;
namespace GameServer.Networking;

public class Client
{
    private readonly TcpClient _client;
    private readonly Server _server;
    private StreamReader _reader;
    private StreamWriter _writer;

    public Client (TcpClient client, Server? server)
    {
        _client = client;
        _server = server!;
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
            _writer.WriteLine(NetworkProtocol.CreateMessage("MOVE", "1,2-3,4"));
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
        Console.WriteLine($"Received: {message}");

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
        Console.WriteLine($"[Server] Handling move: {moveData}");
        Console.WriteLine($"Move by client: {moveData}");
        
        string moveMessage = NetworkProtocol.CreateMessage("MOVE", moveData);
        Console.WriteLine($"[Server] Sending move: {moveMessage}");
        
        _server.BroadcastToClient(moveMessage, this);

        //_server.BroadcastToClient($"Move:{moveMessage}", this);
        Console.WriteLine("[Server] Switching turn");
        _server.Broadcast($"TURN:");
    }


    void HandleChat(string message)
    {
        
    }
    
    public void Send(string message)
    {
        try
        {
            Console.WriteLine($"[Server] Sending to client: {message}");
            _writer.WriteLine(message);
            _writer.Flush();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Send error: {ex.Message}");
        }
    }
}