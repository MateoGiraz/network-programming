namespace IRepository;
using CoreBusiness;

public interface IRepositoryProduct
{
    public void AddProduct(Product product);
    public void RemoveProduct(Product product);
    public Product GetProduct(String name);
    public List<Product> GetProducts();
}