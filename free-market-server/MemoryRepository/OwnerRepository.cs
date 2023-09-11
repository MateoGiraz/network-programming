using CoreBusiness;
using IRepository;

namespace MemoryRepository;

public class OwnerRepository : IRepositoryOwner
{
    private readonly List<Owner> _owners = new ();
    
    public void AddOwner(Owner owner)
    {
        _owners.Add(owner);
    }

    public void RemoveOwner(Owner owner)
    {
        _owners.Remove(owner);
    }

    public Owner GetOwner(string name)
    {

            var foundOwner = _owners.FirstOrDefault(owner => owner.UserName.Equals(name));
            if (foundOwner is null)
            {
                throw new MemoryRepositoryException("Product was not found.");
            }
            return foundOwner;
    }

    public List<Owner> GetOwners()
    {
        return _owners;
    }

    public bool Exists(string username)
    {
        return (_owners.FirstOrDefault(owner => owner.UserName.Equals(username))!=null);
    }
}

