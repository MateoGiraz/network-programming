namespace CoreBusiness;

public class Owner
{
    public string UserName;
    public string Password;

    public override bool Equals(object? obj)
    {
        if ((obj is null) || ! this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            Owner o = (Owner) obj;
            return (UserName == o.UserName);
        }
    }
}