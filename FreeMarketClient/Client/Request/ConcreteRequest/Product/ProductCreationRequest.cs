using Common.DTO;
using Common.Helpers;

namespace free_market_client.Request.ConcreteRequest.Product;

public class ProductCreationRequest : ProductRequest
{
    private string filePath;
    
    protected override void HandleConcreteProductOperation()
    {
        Console.WriteLine($"Add {ProductDto!.Name}'s Description");
        var description = GetInputData();
        ProductDto.Description = description;

        Console.WriteLine("Type Price");
        var price = GetInputData();
        ProductDto.Price = price;
        
        Console.WriteLine("Type Stock");
        var stock = GetInputData();
        ProductDto.Stock = stock;
        
        Console.WriteLine($"Type {ProductDto!.Name}'s image filePath");
        filePath = GetInputData();
    }

    protected override void HandleImageSending()
    {
        var fileTransferHelper = new FileTransferHelper();
        fileTransferHelper.SendFile(base.Socket, filePath);
    }
}