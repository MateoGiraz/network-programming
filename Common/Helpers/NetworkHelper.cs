using System.Net.Sockets;
using System.Text;

namespace Common.Helpers;
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
    
    public static (int, byte[]) ReceiveData(int length, Socket socket)
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
        return (bytesRead, ByteHelper.ConvertBytesToString(stringReceivedData));
    }
    
    public static (int, int) ReceiveIntData(int length, Socket socket)
    {
        var (bytesRead, intReceivedData) = ReceiveData(length, socket);
        return (bytesRead, ByteHelper.ConvertBytesToInt(intReceivedData));
    }

}
