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
        
        var fileTransferHelper = new FileTransferHelper();
        var imgPath = fileTransferHelper.ReceiveFile(socket);
        
        var productDto = new ProductDTO()
        {
            Name = productMap["Name"].ToString(),
            Description = productMap["Description"].ToString(),
            Price = productMap["Price"].ToString(),
            Stock = productMap["Stock"].ToString(),
            ImageRoute = imgPath
        };
        
        var productToBeCreated = new Product()
        {
            Name = productDto.Name,
            Description = productDto.Description,
            ImageRoute = productDto.ImageRoute,
            Price = int.Parse(productDto.Price),
            Stock = int.Parse(productDto.Stock)
        };

        Console.WriteLine("Received Product: " + productDto.Name);
        Console.WriteLine("Received Description: " + productDto.Description);
        
        var productController = new ProductController(); 
        productController.AddProduct(productToBeCreated);
        
        Console.WriteLine("Product to be Created: " + productToBeCreated.Name);
        Console.WriteLine("Product Description: " + productToBeCreated.Description);
        
    }
}