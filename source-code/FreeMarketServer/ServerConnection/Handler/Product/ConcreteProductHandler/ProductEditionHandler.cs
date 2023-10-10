using BusinessLogic;
using CoreBusiness;

namespace ServerConnection.Handler.Product.ConcreteProductHandler;

public class ProductEditionHandler : ProductHandler
{
    protected override async Task HandleProductSpecificOperationAsync()
    {
        ProductDto!.Description = ProductMap["Description"] as string;
        ProductDto.Price = ProductMap["Price"] as string;
        ProductDto.Stock = ProductMap["Stock"] as string;
        ProductDto.ImageRoute = ProductMap["ImageRoute"] as string;

        
        var productToBeEdited = new CoreBusiness.Product()
        {
            Name = ProductDto.Name,
            Description = ProductDto.Description,
            ImageRoute = ProductDto.ImageRoute,
            Price = int.Parse(ProductDto.Price),
            Stock = int.Parse(ProductDto.Stock),
            Owner = new Owner()
            {
                UserName = UserDto!.UserName
            }
        };
        
        var productController = new ProductController(); 
        productController.UpdateProduct(ProductDto.Name, productToBeEdited.Owner, productToBeEdited);
        
        ResponseDto!.StatusCode = 200;
        ResponseDto.Message = $"Updated Product {productToBeEdited.Name}";
    }

}