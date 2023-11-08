using BusinessLogic;
using CoreBusiness;
using Grpc.Core;
using protos.product;
using Product = protos.product.Product;

namespace ServerConnection.gRPC;

public class GrpcService : ProductService.ProductServiceBase
{
    public override Task<ProductResponse> BuyProduct(ProductIdentifier request, ServerCallContext context)
    {
        
        return Task.FromResult(new ProductResponse()
        {
            Code = 200,
            Result = "Hello from rpc"
        });
    }

    public override Task<ProductResponse> CreateProduct(Product request, ServerCallContext context)
    {
        var productController = new ProductController();
        var ownerController = new OwnerController();

        try
        {
            ownerController.LogIn(request.Credentials.Username, request.Credentials.Password);
        }
        catch (AuthenticatorException)
        {
            return Task.FromResult(new ProductResponse()
            {
                Code = 401,
                Result = "Wrong username or password"
            });
        }
        
        var productToBeCreated = new CoreBusiness.Product()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            ImageRoute = "./default-image.jpeg",
            Owner = ownerController.GetOwner(request.Credentials.Username)
        };

        productController.AddProduct(productToBeCreated);
        
        return Task.FromResult(new ProductResponse()
        {
            Code = 200,
            Result = $"Created Product {productToBeCreated.Name}"
        });
    }
    
    public override Task<ProductResponse> UpdateProduct(Product request, ServerCallContext context)
    {
        var productController = new ProductController();
        var ownerController = new OwnerController();
        
        try
        {
            ownerController.LogIn(request.Credentials.Username, request.Credentials.Password);
        }
        catch (AuthenticatorException)
        {
            return Task.FromResult(new ProductResponse()
            {
                Code = 401,
                Result = "Wrong username or password"
            });
        }
        
        var productToBeEdited = new CoreBusiness.Product()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            ImageRoute = "./default-image.jpeg",
            Owner = ownerController.GetOwner(request.Credentials.Username)
        };
        
        productController.UpdateProduct(request.Name, ownerController.GetOwner(request.Credentials.Username), productToBeEdited);
        
        return Task.FromResult(new ProductResponse()
        {
            Code = 200,
            Result = $"Updated Product {productToBeEdited.Name}"
        });
    }

    public override Task<ProductResponse> DeleteProduct(ProductIdentifier request, ServerCallContext context)
    {
        var productController = new ProductController();
        var ownerController = new OwnerController();
        
        try
        {
            ownerController.LogIn(request.Credentials.Username, request.Credentials.Password);
        }
        catch (AuthenticatorException)
        {
            return Task.FromResult(new ProductResponse()
            {
                Code = 401,
                Result = "Wrong username or password"
            });
        }
        
        var toBeDeletedProduct = new CoreBusiness.Product()
        {
            Name = request!.Name,
            Owner = ownerController.GetOwner(request.Credentials.Username)
        };
        
        productController.RemoveProduct(toBeDeletedProduct, toBeDeletedProduct.Owner);
        
        return Task.FromResult(new ProductResponse()
        {
            Code = 200,
            Result = $"Removed Product {toBeDeletedProduct.Name}"
        });    
    }

    public override Task<RatingResponse> GetRating(ProductIdentifier request, ServerCallContext context)
    {
        return Task.FromResult(new RatingResponse()
        {
        });
    }
}