using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;
using Common.Protocol;

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

        while (receivedMessage is not "exit")
        {
            try
            {
                var (bytesRead, messageLength) =
                    NetworkHelper.ReceiveIntData(Protocol.SizeMessageDefinedLength, acceptedConnection);

                if (bytesRead == 0)
                    break;

                (bytesRead, receivedMessage) = NetworkHelper.ReceiveStringData(messageLength, acceptedConnection);

                if (bytesRead == 0)
                    break;

                Console.WriteLine($"Received data from {acceptedConnection.RemoteEndPoint} is {receivedMessage}");
                KOI.PrintEncoded(receivedMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                break;
            }
        }
    }
    
}