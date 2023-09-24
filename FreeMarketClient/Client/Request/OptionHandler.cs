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
        private PicSendingRequest _picSendingRequest;
        private UserCreationRequest _userCreationRequest;

        public OptionHandler(Socket socket)
        {
            _socket = socket;

            //hacer una factory o algo asi?
            _picSendingRequest = new PicSendingRequest();
            _userCreationRequest = new UserCreationRequest();
            _productCreationRequest = new ProductCreationRequest();
        }

        public void Handle(int option)
        {
           switch (option)
            {
                case 1:
                    _userCreationRequest.Handle(_socket, option);
                    break;
                case 2:
                    _picSendingRequest.Handle(_socket, option);
                    break;
                case 3:
                    _productCreationRequest.Handle(_socket,option);
                    break;
                default:
                    break;
            }
        }
    }
}
