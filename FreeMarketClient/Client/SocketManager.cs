using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace free_market_client
{
    internal class SocketManager
    {
        internal static Socket Create()
        {
            Socket socket = new(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);

            var localEndpoint = new IPEndPoint(IPAddress.Parse(ProtocolStandards.LocalHostIp), ProtocolStandards.ClientPort);
            socket.Bind(localEndpoint);

            var serverEndPoint = new IPEndPoint(IPAddress.Parse(ProtocolStandards.LocalHostIp), ProtocolStandards.ServerPort);
            socket.Connect(serverEndPoint);
            
            return socket;
        }
    }
}
