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

internal static class ConnectionManager
{
    internal static TcpListener Create(int port)
    {
        ISettingsManager settingsManager = new SettingsManager();

        var serverIpAddress = IPAddress.Parse(settingsManager.Get(ServerConfig.ServerIpConfigKey));
        var serverPort = int.Parse(settingsManager.Get(ServerConfig.ServerPortConfigKey));

        var localEndpoint = new IPEndPoint(serverIpAddress, serverPort);
        var serverTcpClient = new TcpListener(localEndpoint);

        Console.WriteLine($"IP Address: {settingsManager.Get(ServerConfig.ServerIpConfigKey)}");
        Console.WriteLine($"Port: {settingsManager.Get(ServerConfig.ServerPortConfigKey)}");

        Console.WriteLine("Listening for connections");
        serverTcpClient.Start(100);

        return serverTcpClient;
    }
}
