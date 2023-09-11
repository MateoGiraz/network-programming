using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ServerConnection;

public class Server
{
    public void Listen(int port = 3000)
    {
        var serverSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        var localEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        serverSocket.Bind(localEndpoint);

        Console.WriteLine("Listening for connections");
        serverSocket.Listen(100);

        while (true)
        {
            var acceptedConnection = serverSocket.Accept();
            new Thread(() => HandleConnection(acceptedConnection)).Start();
        }
    }

    private void HandleConnection(Socket acceptedConnection)
    {
        var receivedMessage = "";
        Console.WriteLine($"Connected to client: {acceptedConnection.RemoteEndPoint}");
        while (!receivedMessage.Equals("exit"))
        {
            try
            {
                var receivedData = new byte[1024];
                var bytesRead = acceptedConnection.Receive(receivedData);

                if (bytesRead == 0)
                    continue;
                
                receivedMessage = Encoding.Default.GetString(receivedData, 0, bytesRead);
                Console.WriteLine($"Received data from {acceptedConnection.RemoteEndPoint} is {receivedMessage}");
                
                var sendData = Encoding.Default.GetBytes($"Data received from {acceptedConnection.RemoteEndPoint} is: {receivedMessage}");
                acceptedConnection.Send(sendData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}