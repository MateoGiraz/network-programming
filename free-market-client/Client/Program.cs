using System.Net;
using System.Net.Sockets;
using System.Text;
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
        
        var localEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
        client.Bind(localEndpoint);
        
        var serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);
        client.Connect(serverEndPoint);

        var message = "";
        while (message is not "exit")
        {
            Console.WriteLine("Send a message:");
            message = Console.ReadLine();

            if (message!.Length == 0)
                continue;

            var messageLength = ConvertStringToBytes(message).Length;
            
            SendMessage(ConvertIntToBytes(messageLength), client);
            SendMessage(ConvertStringToBytes(message), client);
        }
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }

    private static byte[] ConvertStringToBytes(string message)
    {
        return Encoding.UTF8.GetBytes(message);
    }

    private static byte[] ConvertIntToBytes(int length)
    {
        return BitConverter.GetBytes(length);
    }

    private static void SendMessage(byte[] message, Socket client)
    {
        var size = message.Length;
        var offset = 0;
        while (offset < size)
        {
            var bytesSent = client.Send(message, offset, size, SocketFlags.None);
            if (bytesSent == 0)
                throw new Exception("possible server error");
            offset += bytesSent;
        }
    }
}