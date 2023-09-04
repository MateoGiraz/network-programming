namespace CoreBusiness;

public class Owner
{
    public string userName;
    public string password;

    public override bool Equals(object? obj)
    {
        if ((obj is null) || ! this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else {
            Owner o = (Owner) obj;
            return (userName == o.userName);
        }
    }
}