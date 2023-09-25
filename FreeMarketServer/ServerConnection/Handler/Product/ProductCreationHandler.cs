using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using CoreBusiness;
using BusinessLogic;

namespace ServerConnection.Handler;

public class ProductCreationHandler
{
    internal static void Handle(Socket socket)
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var productString) = NetworkHelper.ReceiveStringData(messageLength, socket);
        Console.WriteLine(productString);
        if (bytesRead == 0)
            return;
        
        var productMap = KOI.Parse(productString);
    
        Console.WriteLine( productMap["Name"] );
        var userMap = KOI.GetObjectMap(productMap["Owner"]);
        var userDto = new UserDTO()
        {
            UserName = userMap["UserName"]
        };
        
        var productDto = new ProductDTO()
        {
            Name = productMap["Name"] as string,
            Description = productMap["Description"] as string,
            Price = productMap["Price"] as string,
            Stock = productMap["Stock"] as string,
            Owner = userDto
        };
        
        var productToBeCreated = new Product()
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = int.Parse(productDto.Price),
            Stock = int.Parse(productDto.Stock),
            Owner = new Owner()
            {
                UserName = userDto.UserName
            }
        };

        Console.WriteLine("Received Product: " + productDto.Name);
        Console.WriteLine("Received Description: " + productDto.Description);
        
        var productController = new ProductController(); 
        productController.AddProduct(productToBeCreated);
        
        Console.WriteLine("Product to be Created: " + productToBeCreated.Name);
        Console.WriteLine("Product Description: " + productToBeCreated.Description);

        var products = productController.GetProducts();

        Console.WriteLine("Products in system: ");
        foreach (var prod in products)
        {
            Console.WriteLine("prod: " + prod.Name + " of: " + prod.Owner.UserName);
        }

    }
}