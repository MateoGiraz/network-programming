using System.Net.Sockets;
using System.Reflection.Metadata;
using Common.Helpers;
using Common.Protocol;
using Common.DTO;

namespace free_market_client.Request;

public abstract class RequestTemplate
{
    internal ResponseDTO? ResponseDto;
    internal void Handle(Socket socket, int option, string? userName)
    {
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
    
    internal void GetServerResponse(Socket socket)
    {
        var (bytesRead, responseLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var responseString) = NetworkHelper.ReceiveStringData(responseLength, socket);

        if (bytesRead == 0)
            return;

        var responseMap = KOI.Parse(responseString);
        var statusCodeValue = responseMap["StatusCode"] as string;
        var messageValue = responseMap["Message"] as string;
        
        ResponseDto = new ResponseDTO()
        {
            StatusCode = int.Parse(statusCodeValue ?? "500"),
            Message = messageValue ?? "Internal Server Error"
        };

        Console.Clear();
        Console.WriteLine(ResponseDto.Message);
        Thread.Sleep(1500);
    }

}