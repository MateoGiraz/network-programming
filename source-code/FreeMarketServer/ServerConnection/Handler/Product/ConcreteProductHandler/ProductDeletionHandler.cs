using BusinessLogic;
using CoreBusiness;

namespace ServerConnection.Handler.Product.ConcreteProductHandler;

public class ProductDeletionHandler : ProductHandler
{
    protected override async Task HandleProductSpecificOperationAsync()
    {
        var toBeDeletedProduct = new CoreBusiness.Product()
        {
            Name = ProductDto!.Name,
            Owner = new Owner()
            {
                UserName = UserDto!.UserName
            }
        };
        
        var productController = new ProductController(); 
        productController.RemoveProduct(toBeDeletedProduct, toBeDeletedProduct.Owner);
        
        ResponseDto!.StatusCode = 200;
        ResponseDto.Message = $"Removed Product {toBeDeletedProduct.Name}";
    }
}