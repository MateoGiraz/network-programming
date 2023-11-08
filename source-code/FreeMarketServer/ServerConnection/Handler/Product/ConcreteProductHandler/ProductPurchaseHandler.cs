using BusinessLogic;
using ServerConnection.AMQP;
using System.Text.Json;
using ServerConnection.gRPC;

namespace ServerConnection.Handler.Product.ConcreteProductHandler;

public class ProductPurchaseHandler : ProductHandler
{
    public struct Sale
    {
        public string User { get; set; }
        public string Product { get; set; }
    }

    private static TopicsQueueProvider? topicsQueueProvider;

    protected override async Task HandleProductSpecificOperationAsync()
    {
        var productController = new ProductController();
        var purchasedProduct = productController.GetProduct(ProductDto!.Name);
        productController.BuyProduct(purchasedProduct, 1);
        
        var (hasError, message) = CreateProductSale(purchasedProduct, UserDto.UserName);
        
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
        var mailServiceResult = SendProductSale(saleJson);

        Console.WriteLine(mailServiceResult ? "Sent purchase mail to user {0}" : "Failed to send purchase mail to user {0}",
            sale.User);

    }

    public static (bool, string) CreateProductSale(CoreBusiness.Product product, string username)
    {
        var task = CreateProductSaleAsync(product, username);
        task.Wait();
        return task.Result;
    }
    
    private static async Task<(bool, string)> CreateProductSaleAsync(CoreBusiness.Product product, string username)
    {
        var grpcProvider = new GrpcProvider();
        var res = await grpcProvider.CreateSaleAsync(product, username);
        return res;
    }
    
    public static bool SendProductSale(string jsonSale)
    {
        var task = SendProductSaleAsync(jsonSale);
        task.Wait();
        return task.Result;
    }
    
    private static async Task<bool> SendProductSaleAsync(string jsonSale)
    {
        topicsQueueProvider ??= new TopicsQueueProvider();
        return await topicsQueueProvider!.SendMessage(jsonSale);
    }
}