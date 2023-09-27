using CoreBusiness;
using IRepository;
using System.Collections.Generic;
using System.Linq;

namespace MemoryRepository
{
    public sealed class OwnerRepository : IRepositoryOwner
    {
        private static OwnerRepository _instance;
        private static readonly object _instanceLock = new object();
        private readonly object _operationLock = new object();  // Bloqueo para operaciones
        private readonly List<Owner> _owners = new List<Owner>();

        private OwnerRepository() { }  // Constructor privado

        public static OwnerRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new OwnerRepository();
                        }
                    }
                }
                return _instance;
            }
        }

        public void AddOwner(Owner owner)
        {
            lock (_operationLock)
            {
                _owners.Add(owner);
            }
        }

        public void RemoveOwner(Owner owner)
        {
            lock (_operationLock)
            {
                _owners.Remove(owner);
            }
        }

        public Owner GetOwner(string name)
        {
            lock (_operationLock)
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
            lock (_operationLock)
            {
                return new List<Owner>(_owners);  // Devolvemos una copia para que no se pueda modificar la lista original desde fuera
            }
        }

        public bool Exists(string username)
        {
            lock (_operationLock)
            {
                return _owners.Any(owner => owner.UserName.Equals(username));
            }
        }
    }
}



