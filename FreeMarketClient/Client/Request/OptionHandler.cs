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
        private PicSendingRequest _picSendingRequest;
        private UserCreationRequest _userCreationRequest;

        public OptionHandler(Socket socket)
        {
            _socket = socket;

            //hacer una factory o algo asi?
            _picSendingRequest = new PicSendingRequest();
            _userCreationRequest = new UserCreationRequest();
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
                    OptionsLoggedIn("Pepe");
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
            while (res != 6)
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
                    Console.WriteLine("Son Feas Cosas");
                    Thread.Sleep(1500);
                    break;
                case 2:
                    Console.WriteLine("que Cojones");
                    Thread.Sleep(1500);
                    break;
                case 6:
                    Console.WriteLine("Or");
                    break;
                default:
                    Console.WriteLine("That's not a valid Option");
                    Thread.Sleep(1500);
                    break;
                
            }
        }
    }
}
