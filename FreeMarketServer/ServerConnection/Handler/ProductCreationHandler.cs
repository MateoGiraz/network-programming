using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using CoreBusiness;
using BusinessLogic;

namespace ServerConnection.Handler;

public class ProductCreationHandler
{
    //Quizás puede ser genérico
    internal static void Handle(Socket socket)
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var productString) = NetworkHelper.ReceiveStringData(messageLength, socket);

        if (bytesRead == 0)
            return;

        var productMap = KOI.Parse(productString);

        var productDTO = new ProductDTO()
        {
            Name = productMap["Name"].ToString(),
            Description = productMap["Description"].ToString(),
            Price = productMap["Price"].ToString(),
            Stock = productMap["Stock"].ToString(),
            
        };
        
        Product productToBeCreated = new Product()
        {
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = int.Parse(productDTO.Price),
            Stock = int.Parse(productDTO.Stock),
        };

        Console.WriteLine("Received Product: " + productDTO.Name);
        Console.WriteLine("Received Description: " + productDTO.Description);
        
        ProductController _productController = new ProductController(); 
        _productController.AddProduct(productToBeCreated);
        
        Console.WriteLine("Product to be Created: " + productToBeCreated.Name);
        Console.WriteLine("Product Description: " + productToBeCreated.Description);
        
    }
}