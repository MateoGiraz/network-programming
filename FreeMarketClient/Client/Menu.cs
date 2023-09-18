namespace free_market_client;
using Common.DTO;

public class Menu
{
    public static void PrintOptions()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Terminal Menu");
        Console.WriteLine("1. Register User");
        Console.WriteLine("_. Exit");

        Console.Write("Please select an option: ");
    }

    public static object? ChooseOption()
    {
        var userInput = Console.ReadLine();

        if (int.TryParse(userInput, out var choice))
        {
            return choice switch
            {
                1 => HandleOption(1),
                2 => HandleOption(2),
                _ => HandleOption(-1)
            };
        }

        return HandleOption(-1);
    }

    private static object? HandleOption(int option)
    {
        switch (option)
        {
            case 1:
                return RegisterUser();
            default:
                return null;
        }
    }

    private static object? RegisterUser()
    {
        Console.WriteLine("Enter username: ");
        var userName = Console.ReadLine();
        Console.WriteLine("Enter Password: ");
        var password = Console.ReadLine();

        UserDTO? userDto = new()
        {
            UserName = userName!,
            Password = password!
        };

        return userDto;
    }
}