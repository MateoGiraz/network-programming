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
        
        while (receivedMessage is not "exit")
        {
            try
            {
                var receiveDataLength = new byte[4];
                var (bytesRead, messageLength) = ReceiveIntData(acceptedConnection, receiveDataLength);
                
                if (bytesRead == 0)
                    break;

                var receiveData = new byte[messageLength];
                (bytesRead, receivedMessage) = ReceiveStringData(acceptedConnection, receiveData);

                if (bytesRead == 0)
                    break;
                
                Console.WriteLine($"Received data from {acceptedConnection.RemoteEndPoint} is {receivedMessage}");

                var sendMessage =
                    ConvertStringToBytes(
                        $"Data received from {acceptedConnection.RemoteEndPoint} is: {receivedMessage}");
                SendMessage(sendMessage, acceptedConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                break;
            }
        }
    }
    
    private static (int, byte[]) ReceiveData(Socket socket, byte[] receiveData)
    {
        var size = receiveData.Length;
        var offset = 0;
        var bytesRead = 0;
        
        while (offset < size)
        {
            bytesRead = socket.Receive(receiveData, offset, size, SocketFlags.None);
            if (bytesRead == 0)
                throw new Exception("possible client error");
            offset += bytesRead;
        }

        return (bytesRead, receiveData);
    }

    private static (int, string) ReceiveStringData(Socket socket, byte[] receiveData)
    {
        var (bytesRead, stringReceivedData) = ReceiveData(socket, receiveData);
        return (bytesRead, Encoding.Default.GetString(stringReceivedData, 0, bytesRead));
    }
    
    private static (int, int) ReceiveIntData(Socket socket, byte[] receiveData)
    {
        var (bytesRead, intReceivedData) = ReceiveData(socket, receiveData);
        return (bytesRead, BitConverter.ToInt32(intReceivedData));
    }

    private static byte[] ConvertStringToBytes(string message)
    {
        return Encoding.UTF8.GetBytes(message);
    }

    private static void SendMessage(Byte[] message, Socket client)
    {
        var size = message.Length;
        var offset = 0;
        while (offset < size)
        {
            var bytesSent = client.Send(message, offset, size, SocketFlags.None);
            if (bytesSent == 0)
                throw new Exception("possible client error");
            offset += bytesSent;
        }
    }
}