using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using ServerConnection.AMQP;

namespace ServerConnection.Handler.Product
{
    public abstract class ProductHandler
    {
        internal ProductDTO? ProductDto;
        internal ResponseDTO? ResponseDto;
        internal UserDTO? UserDto;
        internal Dictionary<string, object>? ProductMap;
        internal NetworkStream stream;
        internal TopicsQueueProvider? topicsQueueProvider;

        protected abstract Task HandleProductSpecificOperationAsync();

        internal async Task HandleAsync(NetworkStream networkStream, TopicsQueueProvider? _topicsQueueProvider = null)
        {
            stream = networkStream;
            topicsQueueProvider = _topicsQueueProvider;

            try
            {
                var (bytesRead, messageLength) =
                    await NetworkHelper.ReceiveIntDataAsync(ProtocolStandards.SizeMessageDefinedLength, stream);

                if (bytesRead == 0)
                    return;

                (bytesRead, var productString) = await NetworkHelper.ReceiveStringDataAsync(messageLength, stream);

                if (bytesRead == 0)
                    return;

                ProductMap = KOI.Parse(productString);
                var userMap = KOI.GetObjectMap(ProductMap["Owner"]);

                UserDto = new UserDTO()
                {
                    UserName = userMap["UserName"] as string
                };

                ProductDto = new ProductDTO()
                {
                    Name = ProductMap["Name"] as string,
                    Owner = UserDto
                };

                ResponseDto = new ResponseDTO();

                await HandleProductSpecificOperationAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                ResponseDto.StatusCode = 400;
                ResponseDto.Message = ex.Message;
            }

            var responseData = KOI.Stringify(ResponseDto);
            var responseMessageLength = ByteHelper.ConvertStringToBytes(responseData).Length;

            await NetworkHelper.SendMessageAsync(ByteHelper.ConvertIntToBytes(responseMessageLength), stream);
            await NetworkHelper.SendMessageAsync(ByteHelper.ConvertStringToBytes(responseData), stream);
        }
    }
}
