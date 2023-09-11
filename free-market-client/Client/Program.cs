using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client;

class Program
{
    static void Main()
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
        while (!message.Equals("exit"))
        {
            Console.WriteLine("Send a message:");
            message = Console.ReadLine();

            if (message is null || message.Length == 0)
                continue;
            
            var byteMessage = Encoding.UTF8.GetBytes(message);
            client.Send(byteMessage);

            var receiveData = new byte[100];
            var bytesRead = client.Receive(receiveData);
            var serverResponse = Encoding.Default.GetString(receiveData, 0, bytesRead);

            Console.WriteLine($"Server response: {serverResponse}");
        }
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }
}