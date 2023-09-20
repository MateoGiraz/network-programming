using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;
using Common.Protocol;
using Microsoft.CSharp.RuntimeBinder;

namespace free_market_client;

public static class Program
{
    public static void Main()
    {
        Socket client = new(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);
        
        var localEndpoint = new IPEndPoint(IPAddress.Parse(Protocol.LocalHostIp), Protocol.ClientPort);
        client.Bind(localEndpoint);
        
        var serverEndPoint = new IPEndPoint(IPAddress.Parse(Protocol.LocalHostIp), Protocol.ServerPort);
        client.Connect(serverEndPoint);
        
        Startup.PrintWelcomeMessageClient();

        var leave = false;
        while (!leave)
        {
            Menu.PrintOptions();
            var res = Menu.ChooseOption();

            if (res is null)
            {
                leave = true;
                continue;
            }
            
            var message = KOI.Stringify(res);
            var messageLength = NetworkHelper.ConvertStringToBytes(message).Length;

            NetworkHelper.SendMessage(NetworkHelper.ConvertIntToBytes(messageLength), client);
            NetworkHelper.SendMessage(NetworkHelper.ConvertStringToBytes(message), client);
        }
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }

}