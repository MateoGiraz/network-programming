using System;
using System.Net.Sockets;
using System.Text;

namespace Common.Helpers
{
    public class NetworkHelper
    {
        public class ServerDisconnectedException : Exception
        {
            public ServerDisconnectedException(string message) : base(message) { }
        }

        public static void SendMessage(byte[] message, Socket client)
        {
            try
            {
                var size = message.Length;
                var offset = 0;
                while (offset < size)
                {
                    var bytesSent = client.Send(message, offset, size - offset, SocketFlags.None);
                    if (bytesSent == 0)
                    {
                        throw new ServerDisconnectedException("Server has disconnected while sending.");
                    }
                    offset += bytesSent;
                }
            }
            catch (ServerDisconnectedException)
            {
                Console.WriteLine("Error: Server has Shutdown.");
                client.Close();
                throw;
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Error: Server has Shutdown.");
                client.Close();
                throw new ServerDisconnectedException("Error: Server has Shutdown.");
            }
        }

        public static (int, byte[]) ReceiveData(int length, Socket socket)
        {
            try
            {
                var receiveData = new byte[length];
                var size = receiveData.Length;
                var offset = 0;
                var bytesRead = 0;

                while (offset < size)
                {
                    bytesRead = socket.Receive(receiveData, offset, size - offset, SocketFlags.None);
                    if (bytesRead == 0)
                    {
                        throw new ServerDisconnectedException("Server has disconnected while receiving.");
                    }
                    offset += bytesRead;
                }

                return (bytesRead, receiveData);
            }
            catch (ServerDisconnectedException ex)
            {
                Console.WriteLine(ex.Message);
                socket.Close();
                return (0, new byte[0]);
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Error: Attempted communication with a closed socket.");
                return (0, new byte[0]);
            }
        }

        public static (int, string) ReceiveStringData(int length, Socket socket)
        {
            var (bytesRead, stringReceivedData) = ReceiveData(length, socket);
            return (bytesRead, ByteHelper.ConvertBytesToString(stringReceivedData));
        }

        public static (int, int) ReceiveIntData(int length, Socket socket)
        {
            var (bytesRead, intReceivedData) = ReceiveData(length, socket);
            return (bytesRead, ByteHelper.ConvertBytesToInt(intReceivedData));
        }
    }
}

