using System.Text.RegularExpressions;

public static class InputHelper
{
    public static string GetValidInput(string promptMessage)
    {
        string input;
        do
        {
            Console.WriteLine(promptMessage);
            input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty or just spaces. Please try again.");
            }
            else if (input.Contains('#'))
            {
                Console.WriteLine("Input cannot contain the '#' character. Please try again.");
                input = string.Empty; 
            }
        } while (string.IsNullOrWhiteSpace(input) || input.Contains('#'));
        return input;
    }

    public static string GetInputWithoutHash(string promptMessage)
    {
        string input;
        do
        {
            Console.WriteLine(promptMessage);
            input = Console.ReadLine()?.Trim(); 
            if (input.Contains('#'))
            {
                Console.WriteLine("Input cannot contain the '#' character. Please try again.");
                input = string.Empty; 
            }
        } while (input.Contains('#'));
        return input;
    }

    public static string GetInputExistingFile(string promptMessage)
    {
        string input;
        do
        {
            Console.WriteLine(promptMessage);
            input = Console.ReadLine()?.Trim(); 
            if (!File.Exists(input))
            {
                Console.WriteLine("File was not found. Please check filepath provided");
            }
        } while (!File.Exists(input));
        return input;
    }
    

    
    public static string GetValidPositiveNumberInput(string promptMessage)
    {
        string input;
        var regex = new Regex(@"^[0-9]\d*$"); 
        do
        {
            Console.WriteLine(promptMessage);
            input = Console.ReadLine()?.Trim();

            if (!regex.IsMatch(input))
            {
                Console.WriteLine("Please enter a valid positive number.");
            }
            else if (input.Contains('#'))
            {
                Console.WriteLine("Input cannot contain the '#' character. Please try again.");
                input = string.Empty; 
            }
        } while (!regex.IsMatch(input) || input.Contains('#'));
        return input;
    }
    
    public static string GetValidRatingInput(string promptMessage)
    {
        string input;
        var regex = new Regex(@"^[0-5]$"); 
        do
        {
            Console.WriteLine(promptMessage);
            input = Console.ReadLine()?.Trim();

            if (!regex.IsMatch(input))
            {
                Console.WriteLine("Please enter a valid number between 0 and 5.");
            }
            else if (input.Contains('#'))
            {
                Console.WriteLine("Input cannot contain the '#' character. Please try again.");
                input = string.Empty; 
            }
        } while (!regex.IsMatch(input) || input.Contains('#'));
        return input;
    }
    
    public static string GetValidStock(string promptMessage)
    {
        string input;
        var regex = new Regex(@"^[1-9]\d*$");

        do
        {
            Console.WriteLine(promptMessage);
            input = Console.ReadLine()?.Trim();

            if (input == "0" || !regex.IsMatch(input))
            {
                Console.WriteLine("Please enter a valid positive number that is not 0.");
            }
            else if (input.Contains('#'))
            {
                Console.WriteLine("Input cannot contain the '#' character. Please try again.");
                input = string.Empty; 
            }
        } while (input == "0"  || !regex.IsMatch(input) || input.Contains('#'));

        return input;
    }
    
}

