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

        public string LogIn(string username, string password)
        {
            string res = "";
            try
            {
                if (_ownerRepository.Exists(username))
                {
                    Authenticator authenticator = new();
                    Owner foundOwner = GetOwner(username);
                    Authenticator.AuthenticateLogIn(foundOwner, password);
                    return "Welcome back, " + username;
                }
                else
                {
                    SignUp(username, password);
                    res=("Welcome, " + username);
                    return res;
                }
            }
            catch (AuthenticatorException ex)
            {
                return ex.Message;
            }
        }

        private void SignUp(string username, string password)
        {
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
                // SendMessageOfError
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
}


