using System.Net.Sockets;
using BusinessLogic;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace ServerConnection.Handler.User;

public abstract class UserHandler
{
    internal UserDTO? UserDto;
    internal ResponseDTO? ResponseDto;
    
    protected abstract void HandleUserSpecificOperation();
    
    internal void Handle(Socket socket)
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var userString) = NetworkHelper.ReceiveStringData(messageLength, socket);

        if (bytesRead == 0)
            return;

        var userMap = KOI.Parse(userString);

        UserDto = new UserDTO()
        {
            UserName = userMap["UserName"] as string,
            Password = userMap["Password"] as string
        };

        ResponseDto = new ResponseDTO();

        try
        {
            HandleUserSpecificOperation();
        }
        catch (AuthenticatorException ex)
        {
            Console.WriteLine(ex.Message);

            ResponseDto.StatusCode = 400;
            ResponseDto.Message = ex.Message;
        }
        
        var responseData = KOI.Stringify(ResponseDto);
        var responseMessageLength = ByteHelper.ConvertStringToBytes(responseData).Length;

        NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(responseMessageLength), socket);
        NetworkHelper.SendMessage(ByteHelper.ConvertStringToBytes(responseData), socket);
    }
}
