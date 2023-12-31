using System;
using System.Net.Sockets;
using Common.Helpers;
using Common.Protocol;
using Common.DTO;

namespace free_market_client.Request
{
    public abstract class RequestTemplate

    {
        internal ResponseDTO? ResponseDto;

        internal async Task HandleAsync(NetworkStream stream, int option, string? userName)
        {
            try
            {
                await SendCmdAsync(stream, option);

                await ConcreteHandleAsync(stream, userName);
            }
            catch (NetworkHelper.ServerDisconnectedException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        internal abstract Task ConcreteHandleAsync(NetworkStream stream, string? userName);

        internal async Task SendDataAsync(NetworkStream stream, string userData)
        {
            try
            {
                await NetworkHelper.SendMessageAsync(ByteHelper.ConvertStringToBytes(userData), stream);
            }
            catch (NetworkHelper.ServerDisconnectedException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        internal async Task SendLengthAsync(NetworkStream stream, int messageLength)
        {
            try
            {
                await NetworkHelper.SendMessageAsync(ByteHelper.ConvertIntToBytes(messageLength), stream);
            }
            catch (NetworkHelper.ServerDisconnectedException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        internal static async Task SendCmdAsync(NetworkStream stream, int res)
        {
            try
            {
                await NetworkHelper.SendMessageAsync(ByteHelper.ConvertIntToBytes(res), stream);
            }
            catch (NetworkHelper.ServerDisconnectedException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        internal async Task GetServerResponseAsync(NetworkStream stream)
        {
            try
            {
                var (bytesRead, responseLength) =
                   await NetworkHelper.ReceiveIntDataAsync(ProtocolStandards.SizeMessageDefinedLength, stream);

                if (bytesRead == 0)
                    return;

                (bytesRead, var responseString) = await NetworkHelper.ReceiveStringDataAsync(responseLength, stream);

                if (bytesRead == 0)
                    return;

                var responseMap = KOI.Parse(responseString);
                var statusCodeValue = responseMap["StatusCode"] as string;
                var messageValue = responseMap["Message"] as string;

                ResponseDto = new ResponseDTO()
                {
                    StatusCode = int.Parse(statusCodeValue ?? "500"),
                    Message = messageValue ?? "Internal Server Error"
                };

                Console.Clear();
                Console.WriteLine(ResponseDto.Message);
                Thread.Sleep(1500);
            }
            catch (NetworkHelper.ServerDisconnectedException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        internal string GetInputData()
        {
            var ret = "";
            while (ret == "" || ret.Contains('#'))
            {
                ret = Console.ReadLine();
            }

            return ret;
        }
    }
}
