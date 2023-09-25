using CoreBusiness;
using IRepository;
using System.Collections.Generic;
using System.Linq;

namespace MemoryRepository
{
    public class OwnerRepository : IRepositoryOwner
    {
        private static readonly Lazy<OwnerRepository> _instance = new Lazy<OwnerRepository>(() => new OwnerRepository());

        public static OwnerRepository Instance => _instance.Value;

        private readonly List<Owner> _owners = new List<Owner>();

        private OwnerRepository() { }  // Constructor privado

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
            return (_owners.FirstOrDefault(owner => owner.UserName.Equals(username)) != null);
        }
    }
}


