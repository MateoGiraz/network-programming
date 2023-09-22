namespace free_market_client;
using Common.DTO;
using Common.Helpers;
using System.Net.Sockets;

public class Menu
{
    public static void PrintOptions()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Terminal Menu");
        Console.WriteLine("1. Register User");
        Console.WriteLine("2. Send Pic");
        Console.WriteLine("_. Exit");

        Console.Write("Please select an option: ");
    }

    public static int ChooseOption()
    {
        var userInput = Console.ReadLine();

        if (int.TryParse(userInput, out var choice))
        {
            return choice;
        }

        return -1;
    }

}