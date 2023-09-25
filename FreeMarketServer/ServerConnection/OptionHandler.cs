using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using System.Net.Sockets;
using ServerConnection.Handler;


namespace ServerConnection;

internal class OptionHandler
{
    private readonly Socket _socket;

    public OptionHandler(Socket socket)
    {
        _socket = socket;
    }

    public void Handle(int option)
    {
        switch (option)
        {
            case 1:
                UserCreationHandler.Handle(_socket);
                break;
            case 2:
                UserCreationHandler.Handle(_socket);
                break;
            case 3:
                ProductCreationHandler.Handle(_socket);
                break;
            default:
                break;
        }
    }
}
