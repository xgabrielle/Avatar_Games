namespace NetworkProtocolLib;

public class NetworkProtocol
{
    public static (string type, string data) ParseMessage(string message)
    {
        var parts = message.Split(':', 2);
        string type = parts[0];
        string data = parts.Length > 1 ? parts[1]: "";
        
        return (type, data);
    }

    public static string CreateMessage(string type, string data)
    {
        return $"{type} : {data}";
    }
}