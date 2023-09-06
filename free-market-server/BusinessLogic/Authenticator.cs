namespace BusinessLogic;
using CoreBusiness;
public class Authenticator
{
    public static void AuthenticateLogIn(Owner owner, String enteredPassword)
    {
        if (owner.Password.Equals(enteredPassword))
        {
            Console.WriteLine("keloke "+owner.UserName);
        }
        else
        {
            Console.WriteLine("Usuario y/o contraseña incorrectas manín");
        }
    }
}