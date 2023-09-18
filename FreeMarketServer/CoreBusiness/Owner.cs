namespace CoreBusiness;

public class Owner
{
    public string UserName {get; set;}
    public string Password { get; set;}

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