using IRepository;
using CoreBusiness;
using System.Collections.Generic;
using System.Linq;

namespace MemoryRepository
{
    public class ProductRepository : IRepositoryProduct
    {
        private static readonly Lazy<ProductRepository> _instance = new Lazy<ProductRepository>(() => new ProductRepository());

        public static ProductRepository Instance => _instance.Value;

        private readonly List<Product> _products = new List<Product>();

        // Objeto de bloqueo
        private readonly object _lockObject = new object();

        private ProductRepository() { }

        public void AddProduct(Product product)
        {
            lock (_lockObject)
            {
                _products.Add(product);
            }
        }

        public void RemoveProduct(Product product)
        {
            lock (_lockObject)
            {
                _products.Remove(product);
            }
        }

        public Product GetProduct(string name)
        {
            lock (_lockObject)
            {
                var foundProduct = _products.FirstOrDefault(product => product.Name.Equals(name));
                
                if (foundProduct is null)
                {
                    throw new MemoryRepositoryException("Product was not found.");
                }

                return foundProduct;
            }
        }

        public List<Product> GetProducts()
        {
            lock (_lockObject)
            {
                // Es una buena práctica retornar una copia de la lista para evitar problemas de concurrencia externos
                return _products.ToList();
            }
        }

        public List<string> GetProductsNames()
        {
            lock (_lockObject)
            {
                return _products.Select(product => product.Name).ToList();
            }
        }
    }
}
