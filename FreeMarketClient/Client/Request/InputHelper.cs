using System.Text.RegularExpressions;

public static class InputHelper
{
    public static string GetValidInput(string promptMessage)
    {
        string input;
        do
        {
            Console.WriteLine(promptMessage);
            input = Console.ReadLine()?.Trim(); // Trim para eliminar espacios al principio y al final.

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty or just spaces. Please try again.");
            }
            else if (input.Contains('#'))
            {
                Console.WriteLine("Input cannot contain the '#' character. Please try again.");
                input = string.Empty; // Resetear el input para que entre en el bucle de nuevo.
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
                input = Console.ReadLine()?.Trim(); // Trim para eliminar espacios al principio y al final.
                if (input.Contains('#'))
                {
                    Console.WriteLine("Input cannot contain the '#' character. Please try again.");
                    input = string.Empty; // Resetear el input para que entre en el bucle de nuevo.
                }
            } while (input.Contains('#'));
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
                input = string.Empty; // Resetear el input para que entre en el bucle de nuevo.
            }
        } while (!regex.IsMatch(input) || input.Contains('#'));
        return input;
    }
    
    
}

