namespace MemoryRepository;
using IRepository;
using CoreBusiness;

public class ProductRepository : IRepositoryProduct
{
    private readonly List<Product> _products = new();
    
    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public void RemoveProduct(Product product)
    {
        _products.Remove(product);
    }

    public Product GetProduct(String name)
    {
        var foundProduct = _products.FirstOrDefault(product => product.Name.Equals(name));

        if (foundProduct is null)
        {
            throw new MemoryRepositoryException("Product was not found");
        }

        return foundProduct;
    }

    public List<Product> GetProducts()
    {
        return _products;
    }

}