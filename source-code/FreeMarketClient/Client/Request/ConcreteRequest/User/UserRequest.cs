using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.User;

public class UserRequest : RequestTemplate
{
    internal UserDTO? UserDto;
    internal ResponseDTO? ResponseDto;
    
    internal override async Task ConcreteHandle(NetworkStream stream, string? userName)
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

        await SendLength(stream, messageLength);
        await SendData(stream, userData);

        await GetServerResponse(stream);
    }

    private async Task GetServerResponse(NetworkStream stream)
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
            await Task.Delay(1500);
            //Thread.Sleep(1500);
        }
        catch (NetworkHelper.ServerDisconnectedException ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(0);
        }
    }
}