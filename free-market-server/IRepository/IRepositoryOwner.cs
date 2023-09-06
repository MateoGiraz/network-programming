namespace IRepository;
using CoreBusiness;
public interface IRepositoryOwner
{

        public void AddOwner(Owner owner);
        public void RemoveOwner(Owner owner);
        public Owner GetOwner(String name);
        public List<Owner> GetOwners();
        public Boolean Exists(String username);
}