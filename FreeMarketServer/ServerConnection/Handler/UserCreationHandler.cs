using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace ServerConnection.Handler;

public class UserCreationHandler
{
    internal static void Handle(Socket socket)
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var userString) = NetworkHelper.ReceiveStringData(messageLength, socket);

        if (bytesRead == 0)
            return;

        var userMap = KOI.Parse(userString);

        var userDTO = new UserDTO()
        {
            UserName = userMap["UserName"].ToString(),
            Password = userMap["Password"].ToString()
        };
        

        Console.WriteLine("Received Username: " + userDTO.UserName);
        Console.WriteLine("Received Password: " + userDTO.Password);
    }
}