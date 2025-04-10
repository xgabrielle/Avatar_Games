using System.Net.Sockets;

namespace GameServer.Networking;

public class Client
{
    private TcpClient _client;
    private Server _server;
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
            _writer.WriteLine("Hello from client");
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