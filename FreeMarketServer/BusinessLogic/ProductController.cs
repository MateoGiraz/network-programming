using IRepository;
using CoreBusiness;
using MemoryRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessLogic
{
    public class ProductController
    {
        // Accedemos a la instancia Singleton directamente
        private readonly IRepositoryProduct _productRepository = ProductRepository.Instance;

        public void AddProduct(Product product)
        {
            _productRepository.AddProduct(product);
        }

        public void RemoveProduct(Product product, Owner owner)
        {
            var products = _productRepository.GetProducts();

            var deleteProduct = products.FirstOrDefault(toCheckProduct => toCheckProduct.Equals(product));
            
            var type = deleteProduct.GetType();

            foreach (var property in type.GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(deleteProduct);

                Console.WriteLine($"{propertyName}: {propertyValue}");
            }

            if (deleteProduct is null)
            {
                throw new NullReferenceException("Product was not found");
            }
            
            if (!deleteProduct.Owner.Equals(owner))
            {
                throw new ArgumentException("User cannot delete a product they do not own");
            }

            var filePath = deleteProduct.ImageRoute;

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine("File deleted successfully.");
                }
                catch (IOException e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
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
                RemoveProduct(product, product.Owner);
                return;
            }

            product.Stock -= boughtProducts;
        }

        public void UpdateProduct(string name, Owner editor, Product newProduct)
        {
            Product oldProduct = GetProduct(name);
            if (oldProduct.Owner.Equals(editor))
            {
                newProduct.Stock = oldProduct.Stock;
                _productRepository.RemoveProduct(oldProduct);
                _productRepository.AddProduct(newProduct);
                Console.WriteLine("Product " + newProduct.Name + " has been successfully updated");
            }
            else
            {
                Console.WriteLine("You must be the owner of this product (" + name + ") to be able to update it.");
            }
        }

        public List<string> GetProductsNames()
        {
            return _productRepository.GetProductsNames();
        }
    }
}
