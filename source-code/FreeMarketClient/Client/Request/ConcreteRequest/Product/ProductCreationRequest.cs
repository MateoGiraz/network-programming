using Common.DTO;
using Common.Helpers;

namespace free_market_client.Request.ConcreteRequest.Product;

public class ProductCreationRequest : ProductRequest
{
    private string filePath;
    
    protected override async Task HandleConcreteProductOperation()
    {
        ProductDto.Description = InputHelper.GetInputWithoutHash($"Add {ProductDto!.Name}'s Description");
        
        ProductDto.Price = InputHelper.GetValidPositiveNumberInput("Type Price");
        
        ProductDto.Stock = InputHelper.GetValidStock("Type Stock");
        
        filePath = InputHelper.GetValidInput("Type new Image Path");
    }

    protected override async Task HandleImageSending()
    {
        var fileTransferHelper = new FileTransferHelper();
        await fileTransferHelper.SendFileAsync(filePath, base.Stream);
    }
}