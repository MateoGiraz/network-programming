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
        var products = _productRepository.GetProducts();

        var deleteProduct = products.FirstOrDefault(toCheckProduct => toCheckProduct.Equals(product));
        
        if (deleteProduct is null)
        {
            throw new NullReferenceException("Product was not found");
        }
        
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

    public void BuyProduct(Product product, int boughtProducts)
    {
        
        if (product.Stock < boughtProducts)
        {
            throw new ArgumentException("There are not enough products");
        }

        if (product.Stock == boughtProducts)
        {
            RemoveProduct(product);
            return;
        }

        product.Stock -= boughtProducts;
    }

    public void UpdateProduct(String name,Owner editor, Product newProduct)
    {
        Product oldProduct = GetProduct(name);
        if (oldProduct.Owner.Equals(editor))
        {
            newProduct.Stock = oldProduct.Stock;
            _productRepository.RemoveProduct(oldProduct);
            _productRepository.AddProduct(newProduct);
            Console.WriteLine("Wei Ferb tenemos que hacer una playa pa nosotros");
            
        }
        else
        {
            Console.WriteLine("Esta vaina no es tuia pai");
            
        }
        
        
    }
}