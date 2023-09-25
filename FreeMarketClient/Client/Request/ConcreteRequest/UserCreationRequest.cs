using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest;

public class UserCreationRequest : RequestTemplate
{
    public UserDTO LogInUserDto =new UserDTO()
    {
        UserName = "NotValid",
        Password = "14"
    } ;
    
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
        
        // Ahora, espera y recibe la respuesta del servidor
        var (bytesRead, responseLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var userString) = NetworkHelper.ReceiveStringData(responseLength, socket);

        if (bytesRead == 0)
            return;

        var userMap = KOI.Parse(userString);

        var responseDTO = new UserDTO()
        {
            UserName = userMap["UserName"].ToString(),
            Password = userMap["Password"].ToString()
        };

        if (!responseDTO.Password.Equals("Invalid credentials, please try again."))
        {
            LogInUserDto = responseDTO;
            if (bytesRead > 0)
            {
                Console.WriteLine("Server response: " + responseDTO.UserName);
                Console.WriteLine("Server message: " + responseDTO.Password);
            }
        }
    }

}