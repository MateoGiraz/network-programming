using BusinessLogic;
using ServerConnection.AMQP;
using System.Text.Json;

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
        
        ResponseDto!.StatusCode = 200;
        ResponseDto.Message = $"Bought Product {purchasedProduct.Name}, new stock is: {purchasedProduct.Stock}";

        var sale = new Sale
        {
            User = UserDto.UserName,
            Product = purchasedProduct.Name
        };

        string saleJSON = JsonSerializer.Serialize(sale);
        string routingKey = "mails_topic";

        var result = await topicsQueueProvider!.SendMessage(saleJSON, routingKey);

        if (!result)
            Console.WriteLine("Failed to send purchase mail to user {0}", sale.User);
    }
}