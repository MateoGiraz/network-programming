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

        internal void Handle(Socket socket, int option, string? userName)
        {
            try
            {
                SendCmd(socket, option);

                ConcreteHandle(socket, userName);
            }
            catch (NetworkHelper.ServerDisconnectedException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        internal abstract void ConcreteHandle(Socket socket, string? userName);

        internal void SendData(Socket socket, string userData)
        {
            try
            {
                NetworkHelper.SendMessage(ByteHelper.ConvertStringToBytes(userData), socket);
            }
            catch (NetworkHelper.ServerDisconnectedException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        internal void SendLength(Socket socket, int messageLength)
        {
            try
            {
                NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(messageLength), socket);
            }
            catch (NetworkHelper.ServerDisconnectedException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        internal static void SendCmd(Socket socket, int res)
        {
            try
            {
                NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(res), socket);
            }
            catch (NetworkHelper.ServerDisconnectedException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        internal void GetServerResponse(Socket socket)
        {
            try
            {
                var (bytesRead, responseLength) =
                    NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

                if (bytesRead == 0)
                    return;

                (bytesRead, var responseString) = NetworkHelper.ReceiveStringData(responseLength, socket);

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
