namespace free_market_client.Request.ConcreteRequest.Product;

public class ProductEditionRequest : ProductRequest
{
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
        
        Console.WriteLine("Type new Image Path");
        var path = GetInputData();
        ProductDto.ImageRoute = path;    
    }
}