using CoreBusiness;
using IRepository;
using System.Collections.Generic;
using System.Linq;

namespace MemoryRepository
{
    public class OwnerRepository : IRepositoryOwner
    {
        private static readonly Lazy<OwnerRepository> _instance = new Lazy<OwnerRepository>(() => new OwnerRepository());
        
        private readonly object _lockObj = new object();


        public static OwnerRepository Instance => _instance.Value;

        private readonly List<Owner> _owners = new List<Owner>();

        private OwnerRepository() { }  // Constructor privado

        public void AddOwner(Owner owner)
        {
            lock (_lockObj)
            {
                _owners.Add(owner);
            }
        }

        public void RemoveOwner(Owner owner)
        {
            lock (_lockObj)
            {
                _owners.Remove(owner);
            }
        }

        public Owner GetOwner(string name)
        {
            lock (_lockObj)
            {
                var foundOwner = _owners.FirstOrDefault(owner => owner.UserName.Equals(name));
                if (foundOwner is null)
                {
                    throw new MemoryRepositoryException("Product was not found.");
                }
                return foundOwner;
            }
        }

        public List<Owner> GetOwners()
        {
            lock (_lockObj)
            {
                return _owners.ToList(); // Retorna una copia para evitar problemas de concurrencia fuera de esta clase
            }
        }

        public bool Exists(string username)
        {
            lock (_lockObj)
            {
                return (_owners.FirstOrDefault(owner => owner.UserName.Equals(username)) != null);
            }
        }

    }
}


