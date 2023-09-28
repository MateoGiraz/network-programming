namespace Common;

public class Startup
{
    private static void PrintWelcomeMessage(string host)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine(@" _____              __  __            _        _");
        Console.WriteLine(@"|  ___| __ ___  ___|  \/  | __ _ _ __| | _____| |_");
        Console.WriteLine(@"| |_ | '__/ _ \/ _ \ |\/| |/ _` | '__| |/ / _ \ __|");
        Console.WriteLine(@"|  _|| | |  __/  __/ |  | | (_| | |  |   <  __/ |_");
        Console.WriteLine(@"|_|  |_|  \___|\___|_|  |_|\__,_|_|  |_|\_\___|\__|");
        Console.WriteLine($"\nWelcome to the Free Market {host}!");
        Console.ResetColor();
        
        Thread.Sleep(1500);
        Console.Clear();
    }

    public static void PrintWelcomeMessageServer()
    {
        PrintWelcomeMessage("Server");
    }
    
    public static void PrintWelcomeMessageClient()
    {
        PrintWelcomeMessage("Client");
    }
}