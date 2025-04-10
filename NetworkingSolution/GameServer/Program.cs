
using System.Net.Sockets;
using GameServer.Networking;

Thread serverThread = new Thread(()=>
{
    Server server = new Server();
    server.OnStart();
});
serverThread.Start();

Thread clientThread = new Thread(() =>
{
    TcpClient tcpClient = new TcpClient("127.0.0.1", 3030);
    Client client = new Client(tcpClient, null);
    client.Handle();

});
clientThread.Start();
