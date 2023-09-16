using IRepository;
using Factory;
using CoreBusiness;
using Common;
using MemoryRepository;
namespace BusinessLogic;


public class OwnerController
{
    private readonly IRepositoryOwner _ownerRepository = OwnerRepositoryCreator.CreateRepository();

    public void LogIn(String username, String password)
    {
        if (_ownerRepository.Exists(username))
        {
            Authenticator authenticator = new();
            Owner foundOwner = GetOwner(username);
            Authenticator.AuthenticateLogIn(foundOwner, password);
        }
        else
        {
            SignUp(username, password);
            Console.WriteLine(username+", bienvenida al clan nenaza");
        }
    }

    private void SignUp(string username, string password)
    {
        KOI KOI = new KOI();
        if (IsStringValid(username) && IsStringValid(password))
        {   
            Owner newOwner = new Owner()
            {
                UserName = username,
                Password = password
            };
            _ownerRepository.AddOwner(newOwner);
        }
        else
        {
            //SendMessageOfError
        }
    }

    private bool IsStringValid(string username)
    {
        if (username.Contains("#")) return false;
        return true;
    }


    public void AddOwner(Owner owner)
    {
        _ownerRepository.AddOwner(owner);
    }

    public void RemoveOwner(Owner owner)
    {
        _ownerRepository.RemoveOwner(owner);
    }

    public Owner GetOwner(string name)
    {
        return _ownerRepository.GetOwner(name);
    }

    public List<Owner> GetOwners()
    {
        return _ownerRepository.GetOwners();
    }
    
}