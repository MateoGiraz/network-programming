using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;
using Common.Protocol;
using Common.Helpers;

namespace ServerConnection;

public class Server
{
    public void Listen(int port = ProtocolStandards.ServerPort)
    {
        var serverSocket = SocketManager.Create(port);

        while (true)
        {
            var acceptedConnection = serverSocket.Accept();
            new Thread(() => HandleConnection(acceptedConnection)).Start();
        }
    }

    private void HandleConnection(Socket acceptedConnection)
    {
        var optionHandler = new OptionHandler(acceptedConnection);
        var receivedMessage = "";
        Console.WriteLine($"Connected to client: {acceptedConnection.RemoteEndPoint}");

        while (receivedMessage is not "exit")
        {
            try
            {
                //TODO: Receive REQ/RES
                
                var (bytesRead, cmd) =
                        NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, acceptedConnection); //TODO: change to cmd len

                if (bytesRead == 0)
                    break;

                optionHandler.Handle(cmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                break;
            }
        }
    }
    
}