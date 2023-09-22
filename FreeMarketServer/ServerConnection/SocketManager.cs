using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerConnection;

internal static class SocketManager
{
    internal static Socket Create(int port)
    {
        var serverSocket = new Socket(
        AddressFamily.InterNetwork,
        SocketType.Stream,
        ProtocolType.Tcp
        );

        var localEndpoint = new IPEndPoint(IPAddress.Parse(ProtocolStandards.LocalHostIp), port);
        serverSocket.Bind(localEndpoint);

        Console.WriteLine("Listening for connections");
        serverSocket.Listen(100);

        return serverSocket;
    }
}
