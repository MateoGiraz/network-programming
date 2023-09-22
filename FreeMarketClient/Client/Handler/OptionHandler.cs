using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using free_market_client.OptionHandler.ConcreteHandlers;

namespace free_market_client.OptionHandler
{
    internal class OptionHandler
    {
        private readonly Socket _socket;
        
        //esto deberia estar aca?
        private PicSendingHandler _picSendingHandler;
        private UserCreationHandler _userCreationHandler;

        public OptionHandler(Socket socket)
        {
            _socket = socket;

            //hacer una factory o algo asi?
            _picSendingHandler = new PicSendingHandler();
            _userCreationHandler = new UserCreationHandler();
        }

        public void Handle(int option)
        {
           switch (option)
            {
                case 1:
                    _userCreationHandler.Handle(_socket, option);
                    break;
                case 2:
                    _picSendingHandler.Handle(_socket, option);
                    break;
                default:
                    break;
            }
        }
    }
}
