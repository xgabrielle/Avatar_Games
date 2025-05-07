using System.Net.Sockets;
using NetworkProtocolLib;
namespace GameServer.Networking;

public class Client
{
    private readonly TcpClient _client;
    private readonly Server _server;
    private readonly string _role;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;
    
    // Track if a move was made
    private bool _moveProcessed = false;
    public Client (TcpClient client, Server? server, string role)
    {
        _client = client;
        _server = server!;
        _role = role;

        var stream = _client.GetStream();
        _reader = new StreamReader(stream);
        _writer = new StreamWriter(stream)
        {
            AutoFlush = true
        };
    }

    public void Handle()
    {
        try
        {
            //_writer.WriteLine($"Player:{_role}");
            
            while (true)
            { 
                string message = _reader.ReadLine();
                if (message == null) break;
                Console.WriteLine($"[{DateTime.Now}] [Server <- {_role}] Received: {message}");
                ProcessMessage(message);
                
                //_server?.BroadcastToClient(message, this);
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine($"Client error: {ex.Message}");
            Console.WriteLine($"[{DateTime.Now}] [Server] Exception: {ex.Message}");

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
        //Console.WriteLine($"Received: {message}");

        switch (type.Trim())
        {
            case "MOVE":
                HandleMove(data);
                break;
            case "CHAT":
                HandleChat(data);
                break;
            default:
                _server.BroadcastToClient(message,this);
                break;
            
        }
    }

    void HandleMove(string moveData)
    {
        if (_server.GetPlayerTurn() != this)
        {
            Send(NetworkProtocol.CreateMessage("ERROR", "Not your turn!"));
            Console.WriteLine($"[{DateTime.Now}] [Server] Move rejected from {_role}: Not their turn.");
            return;
        }
        _moveProcessed = true;
        string moveMessage = NetworkProtocol.CreateMessage("MOVE", moveData);
        
        Send(moveMessage);
        Console.WriteLine($"[{DateTime.Now}] [Server] Sent move confirmation to {_role}: {moveMessage}");
        
        _server.BroadcastToClient(moveMessage, this);
        
        Thread.Sleep(100);
        Console.WriteLine("check what is sent: "+moveMessage);
        
        _server.HandlePlayerTurn();
        var nextTurnPlayer = _server.GetPlayerTurn()._role;
        string turnMessage = NetworkProtocol.CreateMessage("TURN", nextTurnPlayer);
        _server.Broadcast(turnMessage);
        
    }


    void HandleChat(string message)
    {
        
    }
    
    public void Send(string message)
    {
        try
        {
            _writer.WriteLine(message);
            _writer.Flush();
            //Console.WriteLine($"[Server] Sending to client: {message}");
            Console.WriteLine($"[{DateTime.Now}] [Server -> {_role}] Sent: {message}");
        }
        catch (Exception ex)
        {
            //Console.WriteLine($"Send error: {ex.Message}");
            Console.WriteLine($"[{DateTime.Now}] [Send Error] {ex.Message}");
        }
    }

    public string GetRole()
    {
        return _role;
    }
}