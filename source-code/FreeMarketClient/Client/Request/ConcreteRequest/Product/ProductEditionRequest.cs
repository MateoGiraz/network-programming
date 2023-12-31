﻿using Common.Helpers;

namespace free_market_client.Request.ConcreteRequest.Product;

public class ProductEditionRequest : ProductRequest
{
    protected override async Task HandleConcreteProductOperationAsync()
    {
        ProductDto.Description = InputHelper.GetInputWithoutHash($"Add {ProductDto!.Name}'s Description");
        
        ProductDto.Price = InputHelper.GetValidPositiveNumberInput("Type Price");
        
        ProductDto.Stock = InputHelper.GetValidPositiveNumberInput("Type Stock");
        
        ProductDto.ImageRoute = InputHelper.GetValidInput("Type new Image Path");  
    }
    protected override async Task HandleImageSendingAsync() {
        var fileTransferHelper = new FileTransferHelper();
        await fileTransferHelper.SendFileAsync(ProductDto.ImageRoute, base.Stream);
    }
}