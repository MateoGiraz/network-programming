using System.Net.Sockets;
using ServerConnection.Handler.Product.ConcreteProductHandler;
using ServerConnection.Handler.Product.GetProducts;
using ServerConnection.Handler.User;

namespace ServerConnection;

internal class OptionHandler
{
    private readonly NetworkStream _stream;
    private readonly UserCreationHandler _userCreationHandler;
    private readonly UserLogInHandler _userLogInHandler;
    private readonly ProductCreationHandler _productCreationHandler;
    private readonly ProductDeletionHandler _productDeletionHandler;
    private readonly ProductEditionHandler _productEditionHandler;
    private readonly ProductRatingHandler _productRatingHandler;
    private readonly ProductPurchaseHandler _productPurchaseHandler;
    private readonly GetProductsHandler _getProductsHandler;
    private readonly GetProductHandler _getProductHandler;


    public OptionHandler(NetworkStream stream)
    {
        _stream = stream;
        
        _userCreationHandler = new UserCreationHandler();
        _userLogInHandler = new UserLogInHandler();
        _productCreationHandler = new ProductCreationHandler();
        _productDeletionHandler = new ProductDeletionHandler();
        _productEditionHandler = new ProductEditionHandler();
        _productRatingHandler = new ProductRatingHandler();
        _productPurchaseHandler = new ProductPurchaseHandler();
        _getProductsHandler = new GetProductsHandler();
        _getProductHandler = new GetProductHandler();
    }

    public async Task HandleAsync(int option)
    {
        switch (option)
        {
            case 1:
                await _userCreationHandler.HandleAsync(_stream);
                break;
            case 2:
                await _userLogInHandler.HandleAsync(_stream);
                break;
            case 3:
                await _productPurchaseHandler.HandleAsync(_stream);
                break;
            case 4:
                await _productCreationHandler.HandleAsync(_stream);
                break;
            case 5:
                await _productEditionHandler.HandleAsync(_stream);
                break;
            case 6:
                await _productDeletionHandler.HandleAsync(_stream);
                break;
            case 7:
                await _getProductsHandler.HandleAsync(_stream);
                break;
            case 8:
                await _getProductHandler.HandleAsync(_stream);
                break;
            case 9:
                await _productRatingHandler.HandleAsync(_stream);
                break;
            default:
                break;
        }
    }
}
