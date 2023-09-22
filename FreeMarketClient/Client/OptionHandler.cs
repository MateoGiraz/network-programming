using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using System.Net.Sockets;


namespace free_market_client
{
    internal class OptionHandler
    {
        private readonly Socket _socket;

        public OptionHandler(Socket socket)
        {
            _socket = socket;
        }

        public void Handle(int option)
        {
           switch (option)
            {
                case 1:
                    HandleUserCreation(option);
                    break;
                case 2:
                    HandlePicSending(option);
                    break;
                default:
                    break;
            }
        }

        private void HandleUserCreation(int option)
        {
            SendCmd(_socket, option);

            Console.WriteLine("Type Username");
            var user = Console.ReadLine();

            Console.WriteLine("Type Password");
            var password = Console.ReadLine();

            var userDTO = new UserDTO()
            {
                UserName = user,
                Password = password
            };

            var userData = KOI.Stringify(userDTO);
            var messageLength = ByteHelper.ConvertStringToBytes(userData).Length;

            SendLength(messageLength);
            SendData(userData);
        }

        private void HandlePicSending(int option)
        {
            SendCmd(_socket, option);

            Console.WriteLine("Type File Path");
            var path = Console.ReadLine();

            var fileTransferHelper = new FileTransferHelper();
            fileTransferHelper.SendFile(_socket, path);

        }

        private void SendData(string userData)
        {
            NetworkHelper.SendMessage(ByteHelper.ConvertStringToBytes(userData), _socket);
        }

        private void SendLength(int messageLength)
        {
            NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(messageLength), _socket);
        }

        private static void SendCmd(Socket socket, int res)
        {
            NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(res), socket);
        }

    }
}
