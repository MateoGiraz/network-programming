using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using System.Net.Sockets;
using ServerConnection.Handler;
using ServerConnection.Handler.User;


namespace ServerConnection;

internal class OptionHandler
{
    private readonly Socket _socket;
    private readonly UserCreationHandler _userCreationHandler;
    private readonly UserLogInHandler _userLogInHandler;

    public OptionHandler(Socket socket)
    {
        _socket = socket;
        
        _userCreationHandler = new UserCreationHandler();
        _userLogInHandler = new UserLogInHandler();
    }

    public void Handle(int option)
    {
        switch (option)
        {
            case 1:
                _userCreationHandler.Handle(_socket);
                break;
            case 2:
                _userLogInHandler.Handle(_socket);
                break;
            case 3:
                ProductCreationHandler.Handle(_socket);
                break;
            default:
                break;
        }
    }
}
