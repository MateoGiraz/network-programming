using ServerConnection;
using Common;
using BusinessLogic;
using CoreBusiness;
using MemoryRepository;
using System.Threading;

class Program
{
    static Server server = new Server();

    static void Main(string[] args)
    {
        Startup.PrintWelcomeMessageServer();
        Console.WriteLine("For shutting down all connections, please enter 'shutdown'.");
        
        new Thread(MonitorUserInput).Start();
        
        server.Listen(3000);
    }

    static void MonitorUserInput()
    {
        while (true)
        {
            var input = Console.ReadLine()?.Trim();
            if (input?.ToLower() == "shutdown")
            {
                Console.WriteLine("Shutting down server...");
                server.Stop();
                Environment.Exit(0);
            }
        }
    }
}




