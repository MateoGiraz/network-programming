using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using BusinessLogic;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace ServerConnection.Handler.User
{
    public abstract class UserHandler
    {
        internal UserDTO? UserDto;
        internal ResponseDTO? ResponseDto;

        protected abstract Task HandleUserSpecificOperationAsync();

        internal async Task HandleAsync(NetworkStream stream)
        {
            try
            {
                var (bytesRead, messageLength) =
                    await NetworkHelper.ReceiveIntDataAsync(ProtocolStandards.SizeMessageDefinedLength, stream);

                if (bytesRead == 0)
                    return;

                (bytesRead, var userString) = await NetworkHelper.ReceiveStringDataAsync(messageLength, stream);

                if (bytesRead == 0)
                    return;

                var userMap = KOI.Parse(userString);

                UserDto = new UserDTO()
                {
                    UserName = userMap["UserName"] as string,
                    Password = userMap["Password"] as string
                };

                ResponseDto = new ResponseDTO();

                await HandleUserSpecificOperationAsync();
            }
            catch (AuthenticatorException ex)
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