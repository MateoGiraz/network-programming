using System.Security.Authentication;

namespace BusinessLogic;
using CoreBusiness;
public class Authenticator
{
    public static void AuthenticateLogIn(Owner owner, String enteredPassword)
    {
        if (owner.Password.Equals(enteredPassword))
        {
            Console.WriteLine("Welcome back, "+owner.UserName);
        }
        else
        {
            throw new AuthenticatorException("Invalid credentials, please try again.");
        }
    }
}

