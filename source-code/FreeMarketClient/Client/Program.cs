using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;
using Common.Helpers;
using Common.Protocol;
using free_market_client.Request;
using Microsoft.CSharp.RuntimeBinder;

namespace free_market_client;

public static class Program
{
    private const int exitOption = 3;
    public static void Main()
    {

        var socket = SocketManager.Create();
        var optionHandler = new OptionHandler(socket);

        Startup.PrintWelcomeMessageClient();

        var res = -1;
        while (res != exitOption)
        {
            Menu.PrintOptions();
            res = Menu.ChooseOption();
            optionHandler.Handle(res);
        }
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }


}