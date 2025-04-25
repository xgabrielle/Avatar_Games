using System.Net.Sockets;
using GameServer.Networking;

Thread serverThread = new Thread(()=>
{
    Server server = new Server();
    server.OnStart();
});
serverThread.Start();

