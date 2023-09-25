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
        Console.WriteLine("2. Log in");
        Console.WriteLine("3. Exit");

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



    public static void PrintOptionsLoggedIn(string username)
    {
        Console.Clear();
        Console.WriteLine($"Welcome back {username}");
        Console.WriteLine("1. Purchase a Product");
        Console.WriteLine("2. Create a Product");
        Console.WriteLine("3. Modified a Product");
        Console.WriteLine("4. Drop off a Product");
        Console.WriteLine("5. Get Products");
        Console.WriteLine("6. Consult for a Product by Name");
        Console.WriteLine("7. Rate a Product");
        Console.WriteLine("8. Log out");

        Console.Write("Please select an option : ");
    }


}