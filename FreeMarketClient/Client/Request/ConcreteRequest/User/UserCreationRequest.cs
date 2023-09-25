using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.User;

public class UserCreationRequest : RequestTemplate
{
    internal override void ConcreteHandle(Socket socket)
    {
        Console.WriteLine("Type Username");
        var user = Console.ReadLine();
        
        Console.WriteLine("Type Password");
        var password = Console.ReadLine();
        
        var userDto = new UserDTO()
        {
            UserName = user,
            Password = password
        };

        var userData = KOI.Stringify(userDto);
        var messageLength = ByteHelper.ConvertStringToBytes(userData).Length;
        
        SendLength(socket, messageLength);
        SendData(socket, userData);
        
        // Ahora, espera y recibe la respuesta del servidor
        var (bytesRead, responseLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var responseString) = NetworkHelper.ReceiveStringData(responseLength, socket);

        if (bytesRead == 0)
            return;

        var responseMap = KOI.Parse(responseString);

        var responseDto = new ResponseDTO()
        {
            StatusCode = int.Parse(responseMap["StatusCode"].ToString()),
            Message = responseMap["Message"].ToString()
        };

        Console.Clear();
        Console.WriteLine(responseDto.Message);
        Thread.Sleep(1500);
        
        if (responseDto.StatusCode != 200)
            return;
        
        if (bytesRead > 0)
        {
            Console.WriteLine("Server response: " + responseDto.StatusCode);
            Console.WriteLine("Server message: " + responseDto.Message);
        }
    }

}