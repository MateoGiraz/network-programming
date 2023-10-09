using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class NetworkHelper
    {
        public class ServerDisconnectedException : Exception
        {
            public ServerDisconnectedException(string message) : base(message) { }
        }

        public static async Task SendMessageAsync(byte[] message, NetworkStream stream)
        {
            try
            {
                var size = message.Length;
                await stream.WriteAsync(message, 0, size);
            }
            catch (Exception e) when (e is SocketException or ServerDisconnectedException)
            {
                Console.WriteLine("Error: Server has disconnected while sending.");
                stream.Close();
                throw;
            }
        }

        public static async Task<(int, byte[])> ReceiveDataAsync(int length, NetworkStream stream)
        {
            try
            {
                var receiveData = new byte[length];
                var size = receiveData.Length;
                var offset = 0;

                while (offset < size)
                {
                    offset += await stream.ReadAsync(receiveData, offset, size - offset);
                }

                return (offset, receiveData);
            }
            catch (ServerDisconnectedException)
            {
                Console.WriteLine("Error: Server has disconnected while receiving.");
                stream.Close();
                return (0, Array.Empty<byte>());
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Error: Attempted communication with a closed socket.");
                return (0, Array.Empty<byte>());
            }
        }

        public static async Task<(int, string)> ReceiveStringDataAsync(int length, NetworkStream stream)
        {
            var (bytesRead, data) = await ReceiveDataAsync(length, stream);
            return (bytesRead, Encoding.UTF8.GetString(data));
        }

        public static async Task<(int, int)> ReceiveIntDataAsync(int length, NetworkStream stream)
        {
            var (bytesRead, data) = await ReceiveDataAsync(length, stream);
            return (bytesRead, BitConverter.ToInt32(data, 0));
        }
    }
}