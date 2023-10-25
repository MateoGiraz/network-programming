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
    private const int retryIntervalMilliseconds = 5000;
    public static async Task Main()
    {
        
        TcpClient tcpClient = null;

        while (true)
        {
            try
            {
                tcpClient = ConnectionManager.Create();
                break; 
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Error de conexión");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error de E/S");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inesperado");
            }
           
            Console.WriteLine($"Reintentando en {retryIntervalMilliseconds / 1000} segundos..."); 
            Thread.Sleep(retryIntervalMilliseconds);
        }

        if (tcpClient != null)
        {
            var stream = tcpClient.GetStream();
            var optionHandler = new OptionHandler(stream);

            Startup.PrintWelcomeMessageClient();

            var res = -1;
            while (res != exitOption)
            {
                Menu.PrintOptions();
                res = Menu.ChooseOption();
                await optionHandler.Handle(res);
            }

            tcpClient.Close();
        }
        else
        {
            Console.WriteLine("No se pudo establecer la conexión después de varios intentos. Saliendo del programa.");
        }
    }
}