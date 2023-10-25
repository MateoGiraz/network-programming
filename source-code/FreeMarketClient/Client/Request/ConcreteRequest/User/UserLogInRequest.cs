using System.Net.Sockets;
using Common.DTO;

namespace free_market_client.Request.ConcreteRequest.User;

public class UserLogInRequest : UserRequest
{
    public UserDTO? LogInUserDto;
    
    internal override async Task ConcreteHandleAsync(NetworkStream stream, string? userName)
    {
        await base.ConcreteHandleAsync(stream, userName);
        if(UserDto != null && ResponseDto is { StatusCode: 200 })
            LogInUserDto = UserDto;
    }

}