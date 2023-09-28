using Common.DTO;
using Common.Helpers;

namespace free_market_client.Request.ConcreteRequest.Product;

public class ProductCreationRequest : ProductRequest
{
    private string filePath;
    
    protected override void HandleConcreteProductOperation()
    {
        ProductDto.Description = InputHelper.GetInputWithoutHash($"Add {ProductDto!.Name}'s Description");
        
        ProductDto.Price = InputHelper.GetValidPositiveNumberInput("Type Price");
        
        ProductDto.Stock = InputHelper.GetValidPositiveNumberInput("Type Stock");
        
        filePath = InputHelper.GetValidInput("Type new Image Path");
    }

    protected override void HandleImageSending()
    {
        var fileTransferHelper = new FileTransferHelper();
        fileTransferHelper.SendFile(base.Socket, filePath);
    }
}