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
        private readonly Socket _socket;
        
        //esto deberia estar aca?
        private ProductCreationRequest _productCreationRequest;
        private UserRequest _userRequest;
        private UserLogInRequest _userLogInRequest;
        private ProductRatingRequest _productRatingRequest;
        private ProductEditionRequest _productEditionRequest;
        private ProductDeletionRequest _productDeletionRequest;
        private ProductPurchaseRequest _productPurchaseRequest;
        private GetProductsRequest _getProductsRequest;
        private GetProductRequest _getProductRequest;

        public OptionHandler(Socket socket)
        {
            _socket = socket;

            //hacer una factory o algo asi?
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

        public void Handle(int option)
        {
           switch (option)
            {
                case 1:
                    _userRequest.Handle(_socket, option, null);
                    break;
                case 2:
                    _userLogInRequest.Handle(_socket, option, null);
                    if (_userLogInRequest.LogInUserDto is not null)
                    {
                        OptionsLoggedIn(_userLogInRequest.LogInUserDto);
                    }
                    break;
                default:
                    Console.WriteLine("That's not a valid option");
                    Thread.Sleep(1500);
                    break;
            }
        }

        private void OptionsLoggedIn(UserDTO username)
        {
            Console.WriteLine($"Logged in as {username}");
            var res = -1;
            while (res != 8)
            {
                Menu.PrintOptionsLoggedIn(username.UserName);
                res = Menu.ChooseOption();
                HandleLogIn(res, username.UserName);
            }
        }

        private void HandleLogIn(int option, string userName)
        {
            switch (option)
            {
                case 1:
                    Console.WriteLine("Purchase a Product");
                    _productPurchaseRequest.Handle(_socket, option + 2, userName);
                    break;
                case 2:
                    Console.WriteLine("Create Product");
                    _productCreationRequest.Handle(_socket, option + 2, userName);
                    break;
                case 3:
                    Console.WriteLine("Modify Product");
                    _productEditionRequest.Handle(_socket, option + 2, userName);
                    break;
                case 4:
                    Console.WriteLine("Drop Product");
                    _productDeletionRequest.Handle(_socket, option + 2, userName);
                    break;
                case 5:
                    Console.WriteLine("Get Products");
                    _getProductsRequest.Handle(_socket, option + 2, userName);
                    break;
                case 6:
                    Console.WriteLine("Get a Product by Name");                    
                    _getProductRequest.Handle(_socket, option + 2, userName);
                    break;
                case 7:
                    Console.WriteLine("Rate a Product");
                    _productRatingRequest.Handle(_socket, option + 2, userName);
                    break;
                case 8:
                    Console.WriteLine("Log out");
                    _userLogInRequest.LogInUserDto = null;
                    break;
                default:
                    Console.WriteLine("That's not a valid Option");
                    Thread.Sleep(1500);
                    break;
                
            }
        }
    }
}
