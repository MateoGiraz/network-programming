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
        Console.WriteLine("2. (Ahora Sendea Pics)Publish a Product");
        Console.WriteLine("3. Purchase a Product");
        Console.WriteLine("4. Modified a Product");
        Console.WriteLine("5. Drop off a Product");
        Console.WriteLine("6. Search for a Product");
        Console.WriteLine("7. Consult for a Product by Name");
        Console.WriteLine("9. Exit");

        Console.Write("Please select an option : ");
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