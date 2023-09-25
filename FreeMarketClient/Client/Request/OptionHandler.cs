using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using free_market_client.Request.ConcreteRequest;

namespace free_market_client.Request
{
    internal class OptionHandler
    {
        private readonly Socket _socket;
        
        //esto deberia estar aca?
        private ProductCreationRequest _productCreationRequest;
        private UserCreationRequest _userCreationRequest;
        private ProductRatingRequest _productRatingRequest;
        private ProductEditionRequest _productEditionRequest;
        private ProductDeletionRequest _productDeletionRequest;
        private ProductPurchaseRequest _productPurchaseRequest;

        public OptionHandler(Socket socket)
        {
            _socket = socket;

            //hacer una factory o algo asi?
            _userCreationRequest = new UserCreationRequest();
            _productCreationRequest = new ProductCreationRequest();
            _productRatingRequest = new ProductRatingRequest();
            _productEditionRequest = new ProductEditionRequest();
            _productDeletionRequest = new ProductDeletionRequest();
            _productPurchaseRequest = new ProductPurchaseRequest();
        }

        public void Handle(int option)
        {
           switch (option)
            {
                case 1:
                    _userCreationRequest.Handle(_socket, option);
                    break;
                case 2:
                    //_picSendingRequest.Handle(_socket, option);
                    while (_userCreationRequest.LogInUserDto is null)
                    {
                        _userCreationRequest.Handle(_socket, option);
                        Console.WriteLine("The credentials were not valid, please try");
                        Thread.Sleep(1000);
                    }
                    OptionsLoggedIn(_userCreationRequest.LogInUserDto);
                    break;
                case 3:
                    break;
                default:
                    Console.WriteLine("That's not a valid option");
                    Thread.Sleep(1500);
                    break;
            }
        }

        private void OptionsLoggedIn(string username)
        {
            var res = -1;
            while (res != 8)
            {
                Menu.PrintOptionsLoggedIn(username);
                res = Menu.ChooseOption();
                HandleLogIn(res);
            }
        }

        private void HandleLogIn(int option)
        {
            switch (option)
            {
                case 1:
                    Console.WriteLine("Purchase a Product");
                    _productPurchaseRequest.Handle(_socket, option);
                    break;
                case 2:
                    Console.WriteLine("Create Product");
                    _productCreationRequest.Handle(_socket, option);
                    break;
                case 3:
                    Console.WriteLine("Modify Product");
                    _productEditionRequest.Handle(_socket, option);
                    break;
                case 4:
                    Console.WriteLine("Drop Product");
                    _productDeletionRequest.Handle(_socket, option);
                    break;
                case 5:
                    //TODO
                    Console.WriteLine("Get Products");
                    break;
                case 6:
                    Console.WriteLine("Consult for a Product by Name");                    
                    _productRatingRequest.Handle(_socket, option);
                    break;
                case 7:
                    Console.WriteLine("Rate a Product");
                    _productRatingRequest.Handle(_socket, option);
                    break;
                case 8:
                    Console.WriteLine("Log out");
                    _userCreationRequest.LogInUserDto = null;
                    break;
                default:
                    Console.WriteLine("That's not a valid Option");
                    Thread.Sleep(1500);
                    break;
                
            }
        }
    }
}
