using IRepository;
using CoreBusiness;
using Common;
using MemoryRepository;
using System;

namespace BusinessLogic
{
    public class OwnerController
    {
        // Accedemos a la instancia Singleton directamente
        private readonly IRepositoryOwner _ownerRepository = OwnerRepository.Instance;

        public void LogIn(string username, string password)
        {
            try
            {
                if (!_ownerRepository.Exists(username)) 
                    throw new AuthenticatorException("User does not exists");
                
                var foundOwner = GetOwner(username);
                Authenticator.AuthenticateLogIn(foundOwner, password);
            }
            catch (AuthenticatorException ex)
            {
                throw new AuthenticatorException(ex.Message);
            }

        }

        public void SignUp(string username, string password)
        {
            if (!IsStringValid(username) || !IsStringValid(password))
                throw new AuthenticatorException("UserName & Password must not include hashtag symbol");
            
            var newOwner = new Owner()
            {
                UserName = username,
                Password = password
            };
            _ownerRepository.AddOwner(newOwner);
        }

        private bool IsStringValid(string username)
        {
            return !username.Contains('#');
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
}


