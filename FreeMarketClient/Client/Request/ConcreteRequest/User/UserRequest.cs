using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.User;

public class UserRequest : RequestTemplate
{
    internal UserDTO? UserDto;
    internal ResponseDTO? ResponseDto;
    
    internal override void ConcreteHandle(Socket socket)
    {
        Console.WriteLine("Type Username");
        var user = GetInputData();
        
        Console.WriteLine("Type Password");
        var password = GetInputData();
        
        UserDto = new UserDTO()
        {
            UserName = user,
            Password = password
        };

        var userData = KOI.Stringify(UserDto);
        var messageLength = ByteHelper.ConvertStringToBytes(userData).Length;
        
        SendLength(socket, messageLength);
        SendData(socket, userData);
        
        GetServerResponse(socket);
    }

    private void GetServerResponse(Socket socket)
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