using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common.Config;

namespace ServerConnection;

internal static class SocketManager
{
    internal static Socket Create(int port)
    {
        ISettingsManager settingsManager = new SettingsManager();
        var serverSocket = new Socket(
        AddressFamily.InterNetwork,
        SocketType.Stream,
        ProtocolType.Tcp
        );
        Console.WriteLine($"IP Address: {settingsManager.Get(ServerConfig.ServerIpConfigKey)}");
        Console.WriteLine($"Port: {settingsManager.Get(ServerConfig.ServerPortConfigKey)}");

        var localEndpoint = new IPEndPoint(IPAddress.Parse(settingsManager.Get(ServerConfig.ServerIpConfigKey)), int.Parse(settingsManager.Get(ServerConfig.ServerPortConfigKey)));
        serverSocket.Bind(localEndpoint);

        Console.WriteLine("Listening for connections");
        serverSocket.Listen(100);

        return serverSocket;
    }
}
