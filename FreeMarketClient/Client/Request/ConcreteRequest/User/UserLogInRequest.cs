using System.Net.Sockets;
using Common.DTO;

namespace free_market_client.Request.ConcreteRequest.User;

public class UserLogInRequest : UserRequest
{
    public UserDTO? LogInUserDto;
    
    internal override void ConcreteHandle(Socket socket, string? userName)
    {
        base.ConcreteHandle(socket, userName);
        if(UserDto != null && ResponseDto is { StatusCode: 200 })
            LogInUserDto = UserDto;
    }

}