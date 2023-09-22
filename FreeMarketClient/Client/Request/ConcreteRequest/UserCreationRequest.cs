using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest;

public class UserCreationRequest : RequestTemplate
{
    internal override void ConcreteHandle(Socket socket)
    {
        Console.WriteLine("Type Username");
        var user = Console.ReadLine();

        Console.WriteLine("Type Password");
        var password = Console.ReadLine();

        var userDTO = new UserDTO()
        {
            UserName = user,
            Password = password
        };

        var userData = KOI.Stringify(userDTO);
        var messageLength = ByteHelper.ConvertStringToBytes(userData).Length;

        SendLength(socket, messageLength);
        SendData(socket, userData);
    }
}