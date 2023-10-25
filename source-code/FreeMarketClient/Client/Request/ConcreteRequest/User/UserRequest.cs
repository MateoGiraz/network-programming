using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.User;

public class UserRequest : RequestTemplate
{
    internal UserDTO? UserDto;
    internal ResponseDTO? ResponseDto;
    
    internal override async Task ConcreteHandleAsync(NetworkStream stream, string? userName)
    {
        Console.Clear();
        var user = InputHelper.GetValidInput("Type Username");
        var password = InputHelper.GetValidInput("Type Password");

        
        UserDto = new UserDTO()
        {
            UserName = user,
            Password = password
        };

        var userData = KOI.Stringify(UserDto);
        var messageLength = ByteHelper.ConvertStringToBytes(userData).Length;

        await SendLengthAsync(stream, messageLength);
        await SendDataAsync(stream, userData);

        await GetServerResponseAsync(stream);
    }

    private async Task GetServerResponseAsync(NetworkStream stream)
    {
        try
        {
            var (bytesRead, responseLength) =
                 await NetworkHelper.ReceiveIntDataAsync(ProtocolStandards.SizeMessageDefinedLength, stream);

            if (bytesRead == 0)
                return;

            (bytesRead, var responseString) = await NetworkHelper.ReceiveStringDataAsync(responseLength, stream);

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
        catch (NetworkHelper.ServerDisconnectedException ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(0);
        }
    }
}