using BusinessLogic;
using ServerConnection.AMQP;
using System.Text.Json;
using ServerConnection.gRPC;

namespace ServerConnection.Handler.Product.ConcreteProductHandler;

public class ProductPurchaseHandler : ProductHandler
{
    private struct Sale
    {
        public string User { get; set; }
        public string Product { get; set; }
    }

    protected override async Task HandleProductSpecificOperationAsync()
    {
        var productController = new ProductController();
        var purchasedProduct = productController.GetProduct(ProductDto!.Name);
        productController.BuyProduct(purchasedProduct, 1);
        
        var grpcProvider = new GrpcProvider();
        var (hasError, message) = await grpcProvider.CreateSaleAsync(purchasedProduct, UserDto.UserName);
        
        Console.WriteLine(message);
        if (hasError)
        {
            ResponseDto!.StatusCode = 500;
            ResponseDto.Message = message;
            return;
        }
        
        ResponseDto!.StatusCode = 200;
        ResponseDto.Message = $"Bought Product {purchasedProduct.Name}, new stock is: {purchasedProduct.Stock}";

        var sale = new Sale
        {
            User = UserDto.UserName,
            Product = purchasedProduct.Name
        };

        var saleJson = JsonSerializer.Serialize(sale);
        var mailServiceResult = await topicsQueueProvider!.SendMessage(saleJson);

        Console.WriteLine(mailServiceResult ? "Sent purchase mail to user {0}" : "Failed to send purchase mail to user {0}",
            sale.User);

    }
}