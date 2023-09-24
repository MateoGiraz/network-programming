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
    public static void Main()
    {

        var socket = SocketManager.Create();
        var optionHandler = new OptionHandler(socket);

        Startup.PrintWelcomeMessageClient();

        var res = -1;
        while (res != 3)
        {
            Menu.PrintOptions();
            res = Menu.ChooseOption();
            optionHandler.Handle(res);
        }
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }


}