using IRepository;
using CoreBusiness;
using System.Collections.Generic;
using System.Linq;

namespace MemoryRepository
{
    public class ProductRepository : IRepositoryProduct
    {
        // Creación de una instancia Lazy para el Singleton
        private static readonly Lazy<ProductRepository> _instance = new Lazy<ProductRepository>(() => new ProductRepository());
        
        // Proporcionar un acceso público a esta instancia a través de una propiedad estática
        public static ProductRepository Instance => _instance.Value;

        private readonly List<Product> _products = new List<Product>();

        // Hacer el constructor privado para evitar que otros códigos creen instancias de la clase
        private ProductRepository() { }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            _products.Remove(product);
        }

        public Product GetProduct(string name)
        {
            var foundProduct = _products.FirstOrDefault(product => product.Name.Equals(name));

            if (foundProduct is null)
            {
                throw new MemoryRepositoryException("Product was not found.");
            }

            return foundProduct;
        }

        public List<Product> GetProducts()
        {
            return _products;
        }
    }
}