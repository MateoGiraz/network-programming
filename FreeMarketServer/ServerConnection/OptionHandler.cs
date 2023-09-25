using System.Net.Sockets;
using ServerConnection.Handler.Product.ConcreteProductHandler;
using ServerConnection.Handler.User;

namespace ServerConnection;

internal class OptionHandler
{
    private readonly Socket _socket;
    private readonly UserCreationHandler _userCreationHandler;
    private readonly UserLogInHandler _userLogInHandler;
    private readonly ProductCreationHandler _productCreationHandler;
    private readonly ProductDeletionHandler _productDeletionHandler;
    private readonly ProductEditionHandler _productEditionHandler;
    private readonly ProductRatingHandler _productRatingHandler;
    private readonly ProductPurchaseHandler _productPurchaseHandler;
    private readonly GetProductsHandler _getProductsHandler;


    public OptionHandler(Socket socket)
    {
        _socket = socket;
        
        _userCreationHandler = new UserCreationHandler();
        _userLogInHandler = new UserLogInHandler();
        _productCreationHandler = new ProductCreationHandler();
        _productDeletionHandler = new ProductDeletionHandler();
        _productEditionHandler = new ProductEditionHandler();
        _productRatingHandler = new ProductRatingHandler();
        _productPurchaseHandler = new ProductPurchaseHandler();
        _getProductsHandler = new GetProductsHandler();
    }

    public void Handle(int option)
    {
        switch (option)
        {
            case 1:
                _userCreationHandler.Handle(_socket);
                break;
            case 2:
                _userLogInHandler.Handle(_socket);
                break;
            case 3:
                _productPurchaseHandler.Handle(_socket);
                break;
            case 4:
                _productCreationHandler.Handle(_socket);
                break;
            case 5:
                _productEditionHandler.Handle(_socket);
                break;
            case 6:
                _productDeletionHandler.Handle(_socket);
                break;
            case 7:
                _getProductsHandler.Handle(_socket);
                break;
            case 8:
                //get a prod
                break;
            case 9:
                _productRatingHandler.Handle(_socket);
                break;
            default:
                break;
        }
    }
}
