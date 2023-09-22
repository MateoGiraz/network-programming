using System.Net.Sockets;
using Common.Helpers;

namespace free_market_client.Request.ConcreteRequest;

public class PicSendingRequest : RequestTemplate
{
    internal override void ConcreteHandle(Socket socket)
    {
        Console.WriteLine("Type File Path");
        var path = Console.ReadLine();

        var fileTransferHelper = new FileTransferHelper();
        fileTransferHelper.SendFile(socket, path);
    }
}