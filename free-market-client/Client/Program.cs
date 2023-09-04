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
        var serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);

        using Socket client = new(
            serverEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);

        client.Connect(serverEndPoint);
        Console.WriteLine("Enter your username:");
        var username = Console.ReadLine();

        Console.WriteLine("Enter your password:");
        var password = Console.ReadLine();

        var credentials = new UserCredentials
        {
            Username = username,
            Password = password
        };

        string jsonData = JsonSerializer.Serialize(credentials);

        byte[] sendData = Encoding.UTF8.GetBytes(jsonData);
        client.Send(sendData);

        Console.WriteLine("Credentials sent to the server.");

        byte[] receiveData = new byte[100];
        var bytesRead = client.Receive(receiveData);
        var serverAuthResponseserverAuthResponse = Encoding.Default.GetString(receiveData, 0, bytesRead);

        Console.WriteLine($"Auth server response: {serverAuthResponseserverAuthResponse}");
        client.Close();
    }
}