using BusinessLogic;

namespace ServerConnection.Handler.User;

public class UserLogInHandler : UserHandler
{
    protected override void HandleUserSpecificOperation()
    {
        var oc = new OwnerController();

        if (UserDto == null || ResponseDto == null) 
            return;
        
        oc.LogIn(UserDto.UserName, UserDto.Password);
        ResponseDto.StatusCode = 200;
        ResponseDto.Message = $"Welcome back, {UserDto.UserName}";
    }
}