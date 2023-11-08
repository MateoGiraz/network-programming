using System.Text.Json;
using BusinessLogic;
using Grpc.Core;
using protos.product;
using ServerConnection.Handler.Product.ConcreteProductHandler;
using Product = protos.product.Product;

namespace ServerConnection.gRPC;

public class GrpcService : ProductService.ProductServiceBase
{
    public override Task<ProductResponse> BuyProduct(ProductIdentifier request, ServerCallContext context)
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

        var productToBuy = productController.GetProduct(request.Name);
        try
        {
            productController.BuyProduct(productToBuy, 1);
        }
        catch (Exception e)
        {
            return Task.FromResult(new ProductResponse()
            {
                Code = 401,
                Result = e.Message
            });
        }

        var (hasError, message) =  ProductPurchaseHandler.CreateProductSale(productToBuy, request.Credentials.Username);
        if (hasError)
        {
            return Task.FromResult(new ProductResponse()
            {
                Code = 500,
                Result = message
            });
        }
        
        var sale = new ProductPurchaseHandler.Sale()
        {
            User = request.Credentials.Username,
            Product = request.Name
        };
        var saleJson = JsonSerializer.Serialize(sale);
        var mailServiceResult =  ProductPurchaseHandler.SendProductSale(saleJson);
        
        Console.WriteLine(mailServiceResult ? "Sent purchase mail to user {0}" : "Failed to send purchase mail to user {0}",
            sale.User);
        
        return Task.FromResult(new ProductResponse()
        {
            Code = 200,
            Result = $"Bought product {productToBuy.Name}"
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

        try
        {
            productController.AddProduct(productToBeCreated);
        }
        catch (Exception e)
        {
            return Task.FromResult(new ProductResponse()
            {
                Code = 401,
                Result = e.Message
            });
        }
        
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

        try
        {
            productController.UpdateProduct(request.Name, ownerController.GetOwner(request.Credentials.Username),
                productToBeEdited);
        }
        catch (Exception e)
        {
            return Task.FromResult(new ProductResponse()
            {
                Code = 401,
                Result = e.Message
            });
        }
        
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

        try
        {
            productController.RemoveProduct(toBeDeletedProduct, toBeDeletedProduct.Owner);
        }        
        catch (Exception e)
        {
            return Task.FromResult(new ProductResponse()
            {
                Code = 401,
                Result = e.Message
            });
        }
        
        return Task.FromResult(new ProductResponse()
        {
            Code = 200,
            Result = $"Removed Product {toBeDeletedProduct.Name}"
        });    
    }

    public override Task<RatingResponse> GetRating(ProductIdentifier request, ServerCallContext context)
    {
        var productController = new ProductController();
        var ownerController = new OwnerController();
        
        try
        {
            ownerController.LogIn(request.Credentials.Username, request.Credentials.Password);
        }
        catch (AuthenticatorException)
        {
            return Task.FromResult(new RatingResponse()
            {
                Code = 401,
                Message = "Wrong username or password"
            });
        }
        
        var product = productController.GetProduct(request.Name);
        var response = new RatingResponse();
        
        foreach (var rating in product.Ratings)
        {
                response.Result.Add(new Rating()
                {
                    Comment = rating.Comment,
                    Score = rating.Score
                });
        }

        response.Code = 200;
        return Task.FromResult(response);
    }
}