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
    internal class ConnectionManager
    {
        internal static TcpClient Create()
        {
            
            ISettingsManager settingsManager = new SettingsManager();
            TcpClient client = new TcpClient();
            Console.WriteLine($"IP Address: {IPAddress.Parse(settingsManager.Get("ClientIpAddress"))}");
            Console.WriteLine($"Port: {int.Parse(settingsManager.Get("ClientPort"))}");
            //var localEndpoint = new IPEndPoint(IPAddress.Parse(settingsManager.Get("ClientIpAddress")), int.Parse(settingsManager.Get("ClientPort")));

            var serverEndPoint = new IPEndPoint(IPAddress.Parse(settingsManager.Get("ServerIpAddress")), int.Parse(settingsManager.Get("ServerPort")));
            client.Connect(serverEndPoint);
            
            return client;
        }
    }
}
