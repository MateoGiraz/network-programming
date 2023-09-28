using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Common.Config;

namespace free_market_client
{
    internal class SocketManager
    {
        internal static Socket Create()
        {
            
            ISettingsManager settingsManager = new SettingsManager();
            Socket socket = new(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);
            Console.WriteLine($"IP Address: {IPAddress.Parse(settingsManager.Get("ClientIpAddress"))}");
            Console.WriteLine($"Port: {int.Parse(settingsManager.Get("ClientPort"))}");
            var localEndpoint = new IPEndPoint(IPAddress.Parse(settingsManager.Get("ClientIpAddress")), int.Parse(settingsManager.Get("ClientPort")));
            socket.Bind(localEndpoint);

            var serverEndPoint = new IPEndPoint(IPAddress.Parse(settingsManager.Get("ServerIpAddress")), int.Parse(settingsManager.Get("ServerPort")));
            socket.Connect(serverEndPoint);
            
            return socket;
        }
    }
}
