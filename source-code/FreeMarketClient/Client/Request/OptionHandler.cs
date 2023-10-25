using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using free_market_client.Request.ConcreteRequest;
using free_market_client.Request.ConcreteRequest.Product;
using free_market_client.Request.ConcreteRequest.User;

namespace free_market_client.Request
{
    internal class OptionHandler
    {
        private readonly NetworkStream _stream;
        private const int logOutOption = 8; 
        private ProductCreationRequest _productCreationRequest;
        private UserRequest _userRequest;
        private UserLogInRequest _userLogInRequest;
        private ProductRatingRequest _productRatingRequest;
        private ProductEditionRequest _productEditionRequest;
        private ProductDeletionRequest _productDeletionRequest;
        private ProductPurchaseRequest _productPurchaseRequest;
        private GetProductsRequest _getProductsRequest;
        private GetProductRequest _getProductRequest;

        public OptionHandler(NetworkStream stream)
        {
            _stream = stream;
            
            _userRequest = new UserRequest();
            _userLogInRequest = new UserLogInRequest();
            _productCreationRequest = new ProductCreationRequest();
            _productRatingRequest = new ProductRatingRequest();
            _productEditionRequest = new ProductEditionRequest();
            _productDeletionRequest = new ProductDeletionRequest();
            _productPurchaseRequest = new ProductPurchaseRequest();
            _getProductsRequest = new GetProductsRequest();
            _getProductRequest = new GetProductRequest();
            
        }

        public async Task Handle(int option)
        {
           switch (option)
            {
                case 1:
                    await _userRequest.Handle(_stream, option, null);
                    break;
                case 2:
                    await _userLogInRequest.Handle(_stream, option, null);
                    if (_userLogInRequest.LogInUserDto is not null)
                    {
                        await OptionsLoggedIn(_userLogInRequest.LogInUserDto);
                    }
                    break;
                default:
                    Console.WriteLine("That's not a valid option");
                    Thread.Sleep(1500);
                    break;
            }
        }

        private async Task OptionsLoggedIn(UserDTO username)
        {
            Console.WriteLine($"Logged in as {username}");
            var res = -1;
            while (res != logOutOption)
            {
                Menu.PrintOptionsLoggedIn(username.UserName);
                res = Menu.ChooseOption();
                await HandleLogIn(res, username.UserName);
            }
        }

        private async Task HandleLogIn(int option, string userName)
        {
            switch (option)
            {
                case 1:
                    Console.WriteLine("Purchase a Product");
                    await _productPurchaseRequest.Handle(_stream, option + 2, userName);
                    break;
                case 2:
                    Console.WriteLine("Create Product");
                    await _productCreationRequest.Handle(_stream, option + 2, userName);
                    break;
                case 3:
                    Console.WriteLine("Modify Product");
                    await _productEditionRequest.Handle(_stream, option + 2, userName);
                    break;
                case 4:
                    Console.WriteLine("Drop Product");
                    await _productDeletionRequest.Handle(_stream, option + 2, userName);
                    break;
                case 5:
                    Console.WriteLine("Get Products");
                    await _getProductsRequest.Handle(_stream, option + 2, userName);
                    break;
                case 6:
                    Console.WriteLine("Get a Product by Name");
                    await_getProductRequest.Handle(_stream, option + 2, userName);
                    break;
                case 7:
                    Console.WriteLine("Rate a Product");
                    await _productRatingRequest.Handle(_stream, option + 2, userName);
                    break;
                case 8:
                    Console.WriteLine("Log out");
                    await _userLogInRequest.LogInUserDto = null;
                    break;
                default:
                    Console.WriteLine("That's not a valid Option");
                    Thread.Sleep(1500);
                    break;
                
            }
        }
    }
}
