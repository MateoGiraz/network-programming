using System.Net.Sockets;
using System.Reflection.Metadata;
using Common.Helpers;

namespace free_market_client.Request;

public abstract class RequestTemplate
{
    internal void Handle(Socket socket, int option, string? userName)
    {
        //SEND REQ
        
        SendCmd(socket, option);
        
        ConcreteHandle(socket, userName);
    }
    
    internal abstract void ConcreteHandle(Socket socket, string? userName);
    
    internal void SendData(Socket socket, string userData)
    {
        NetworkHelper.SendMessage(ByteHelper.ConvertStringToBytes(userData), socket);
    }

    internal void SendLength(Socket socket, int messageLength)
    {
        NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(messageLength), socket);
    }

    internal static void SendCmd(Socket socket, int res)
    {
        NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(res), socket);
    }

    internal string GetInputData()
    {
        var ret = "";
        while (ret == "" || ret.Contains('#'))
        {
            ret = Console.ReadLine();
        }

        return ret;
    }

}