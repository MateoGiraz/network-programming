using ServerConnection;
using Common;
using ServerConnection.gRPC.protos;

class Program
{
    static Server server = new Server();

    static async Task Main(string[] args)
    {
        Startup.PrintWelcomeMessageServer();
        Console.WriteLine("For shutting down all connections, please enter 'shutdown'.");
        
        var monitorUser = Task.Run(async () => MonitorUserInput());

        var grpcServer = new GrpcServer();
        var grpcService = Task.Run(() => grpcServer.Listen());
        
        await server.ListenAsync(3000);
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




