using IRepository;
using MemoryRepository;

namespace Factory;

public class OwnerRepositoryCreator
{
    public static IRepositoryOwner CreateRepository()
    {
        return new OwnerRepository();
    }
}