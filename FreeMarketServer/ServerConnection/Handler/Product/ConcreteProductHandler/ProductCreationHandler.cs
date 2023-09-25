using CoreBusiness;
using BusinessLogic;

namespace ServerConnection.Handler.Product.ConcreteProductHandler;

public class ProductCreationHandler : ProductHandler
{
    protected override void HandleProductSpecificOperation()
    {
        ProductDto!.Description = ProductMap["Description"] as string;
        ProductDto.Price = ProductMap["Price"] as string;
        ProductDto.Stock = ProductMap["Stock"] as string;
        
        var productToBeCreated = new CoreBusiness.Product()
        {
            Name = ProductDto.Name,
            Description = ProductDto.Description,
            Price = int.Parse(ProductDto.Price),
            Stock = int.Parse(ProductDto.Stock),
            Owner = new Owner()
            {
                UserName = UserDto!.UserName
            }
        };
        
        var productController = new ProductController(); 
        productController.AddProduct(productToBeCreated);
        
        ResponseDto!.StatusCode = 200;
        ResponseDto.Message = $"Created Product {productToBeCreated.Name}";
    }
}