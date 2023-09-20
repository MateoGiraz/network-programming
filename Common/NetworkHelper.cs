using System.Net.Sockets;
using System.Text;

namespace Common;
public class NetworkHelper
{
    public static void SendMessage(byte[] message, Socket client)
    {
        var size = message.Length;
        var offset = 0;
        while (offset < size)
        {
            var bytesSent = client.Send(message, offset, size - offset, SocketFlags.None);
            if (bytesSent == 0)
                throw new Exception("possible client error");
            offset += bytesSent;
        }
    }
    
    private static (int, byte[]) ReceiveData(int length, Socket socket)
    {
        var receiveData = new byte[length];
        var size = receiveData.Length;
        var offset = 0;
        var bytesRead = 0;
        
        while (offset < size)
        {
            bytesRead = socket.Receive(receiveData, offset, size - offset, SocketFlags.None);
            if (bytesRead == 0)
                throw new Exception("possible client error");
            offset += bytesRead;
        }

        return (bytesRead, receiveData);
    }
    
    public static (int, string) ReceiveStringData(int length, Socket socket)
    {
        var (bytesRead, stringReceivedData) = ReceiveData(length, socket);
        return (bytesRead, Encoding.Default.GetString(stringReceivedData, 0, bytesRead));
    }
    
    public static (int, int) ReceiveIntData(int length, Socket socket)
    {
        var (bytesRead, intReceivedData) = ReceiveData(length, socket);
        return (bytesRead, BitConverter.ToInt32(intReceivedData));
    }

    public static byte[] ConvertStringToBytes(string message)
    {
        return Encoding.UTF8.GetBytes(message);
    }
    
    public static byte[] ConvertIntToBytes(int length)
    {
        return BitConverter.GetBytes(length);
    }

}
