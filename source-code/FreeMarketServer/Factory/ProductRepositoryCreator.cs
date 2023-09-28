namespace Factory;
using IRepository;
using MemoryRepository;

public class ProductRepositoryCreator
{
    public static IRepositoryProduct CreateRepository()
    {
        return new ProductRepository();
    }
}