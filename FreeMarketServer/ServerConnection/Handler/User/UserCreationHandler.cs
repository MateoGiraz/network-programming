using System.Net.Sockets;
using BusinessLogic;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using CoreBusiness;

namespace ServerConnection.Handler.User;

public class UserCreationHandler : UserHandler
{
    protected override void HandleUserSpecificOperation()
    {
        var oc = new OwnerController();

        if (UserDto == null || ResponseDto == null) 
            return;
        
        oc.SignUp(UserDto.UserName, UserDto.Password);
        ResponseDto.StatusCode = 200;
        ResponseDto.Message = $"Registered {UserDto.UserName}";
    }
}