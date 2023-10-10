using BusinessLogic;

namespace ServerConnection.Handler.Product.ConcreteProductHandler;

public class ProductPurchaseHandler : ProductHandler
{
    protected override async Task HandleProductSpecificOperationAsync()
    {
        var productController = new ProductController();
        var purchasedProduct = productController.GetProduct(ProductDto!.Name);
        productController.BuyProduct(purchasedProduct, 1);
        
        ResponseDto!.StatusCode = 200;
        ResponseDto.Message = $"Bought Product {purchasedProduct.Name}, new stock is: {purchasedProduct.Stock}";
    }
}