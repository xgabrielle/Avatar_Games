using System.Net.Sockets;

namespace GameServer.Networking;

public class Client
{
    private TcpClient _client;
    private Server _server;
    private StreamReader _reader;
    private StreamWriter _writer;
    private string _role;

    public Client (TcpClient client, Server server, string role) // could remove role
    {
        _client = client;
        _server = server;
        _role = role;
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
                Console.WriteLine($"Received: {message}");
                _server?.Broadcast(message, this);

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