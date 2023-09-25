using System.Net.Sockets;
using BusinessLogic;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using CoreBusiness;

namespace ServerConnection.Handler.User;

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

        var userDto = new UserDTO()
        {
            UserName = userMap["UserName"].ToString(),
            Password = userMap["Password"].ToString()
        };

        var responseDto = new ResponseDTO();
        var oc = new OwnerController();
        
        try
        {
            oc.SignUp(userDto.UserName, userDto.Password);
            responseDto.StatusCode = 200;
            responseDto.Message = $"Registered {userDto.UserName}";
        }
        catch (AuthenticatorException ex)
        {
            Console.WriteLine(ex.Message);
            
            responseDto.StatusCode = 400;
            responseDto.Message = ex.Message;
        };

        var responseData = KOI.Stringify(responseDto);
        var responseMessageLength = ByteHelper.ConvertStringToBytes(responseData).Length;
        
        NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(responseMessageLength), socket);
        NetworkHelper.SendMessage(ByteHelper.ConvertStringToBytes(responseData), socket);
    }

}