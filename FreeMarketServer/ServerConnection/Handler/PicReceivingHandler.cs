using System.Net.Sockets;
using System.Reflection.Metadata;
using Common.Helpers;

namespace ServerConnection.Handler;

public class PicReceivingHandler
{
    internal static void Handle(Socket socket)
    {
        var fileTransferHelper = new FileTransferHelper();
        fileTransferHelper.ReceiveFile(socket);
    }
}