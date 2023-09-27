using IRepository;
using CoreBusiness;
using System.Collections.Generic;
using System.Linq;

namespace MemoryRepository
{
    public sealed class ProductRepository : IRepositoryProduct
    {
        private static ProductRepository _instance;
        private static readonly object _instanceLock = new object();
        private readonly List<Product> _products = new List<Product>();

        // Objeto de bloqueo para operaciones
        private readonly object _operationLock = new object();

        private ProductRepository() { }  // Constructor privado

        public static ProductRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ProductRepository();
                        }
                    }
                }
                return _instance;
            }
        }

        public void AddProduct(Product product)
        {
            lock (_operationLock)
            {
                _products.Add(product);
            }
        }

        public void RemoveProduct(Product product)
        {
            lock (_operationLock)
            {
                _products.Remove(product);
            }
        }

        public Product GetProduct(string name)
        {
            lock (_operationLock)
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
            lock (_operationLock)
            {
                return new List<Product>(_products);  // Devolvemos una copia para evitar problemas de concurrencia externos
            }
        }

        public List<string> GetProductsNames()
        {
            lock (_operationLock)
            {
                return _products.Select(product => product.Name).ToList();
            }
        }
    }
}

