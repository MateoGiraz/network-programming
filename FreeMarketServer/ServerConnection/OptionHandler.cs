using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using System.Net.Sockets;


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
                HandleUserCreation();
                break;
            case 2:
                HandlePicReceiving();
                break;
            default:
                break;
        }
    }

    private void HandleUserCreation()
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, _socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var userString) = NetworkHelper.ReceiveStringData(messageLength, _socket);

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

    private void HandlePicReceiving()
    {
        var fileTransferHelper = new FileTransferHelper();
        fileTransferHelper.ReceiveFile(_socket);
    }

}
