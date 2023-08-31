namespace BusinessLogic;
using IRepository;
using Factory;
using CoreBusiness;

public class ProductController
{
    private readonly IRepositoryProduct _productRepository = ProductRepositoryCreator.CreateRepository();

    public void AddProduct(Product product)
    {
        _productRepository.AddProduct(product);
    }

    public void RemoveProduct(Product product)
    {
        _productRepository.RemoveProduct(product);
    }

    public Product GetProduct(string name)
    {
        return _productRepository.GetProduct(name);
    }

    public List<Product> GetProducts()
    {
        return _productRepository.GetProducts();
    }
    
}