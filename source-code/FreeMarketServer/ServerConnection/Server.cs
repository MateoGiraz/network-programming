using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Config;
using Common.Protocol;
using Common.Helpers;

namespace ServerConnection
{
    public class Server
    {
        private readonly List<TcpClient> _activeConnections = new List<TcpClient>();
        private TcpListener _serverListener;
        private bool _isRunning = true;

        public async Task ListenAsync(int port = ProtocolStandards.ServerPort)
        {
            _serverListener = ConnectionManager.Create(port);

            while (_isRunning)
            {
                try
                {
                    var acceptedConnection = await _serverListener.AcceptTcpClientAsync();

                    lock (_activeConnections)
                    {
                        _activeConnections.Add(acceptedConnection);
                    }

                    var _ = Task.Run(async () => await HandleConnectionAsync(acceptedConnection));
                }
                catch (SocketException ex)
                {
                    if (!_isRunning)
                    {
                        Console.WriteLine("Server is shutting down.");
                    }
                    else
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;

            lock (_activeConnections)
            {
                foreach (var connection in _activeConnections)
                {
                    connection.Close();
                }
                _activeConnections.Clear();
            }

            _serverListener?.Stop();
        }

        private async Task HandleConnectionAsync(TcpClient acceptedConnection)
        {
            var stream = acceptedConnection.GetStream();
            var optionHandler = new OptionHandler(stream);
            var receivedMessage = "";
            Console.WriteLine($"Connected to client: {(IPEndPoint)acceptedConnection.Client.RemoteEndPoint}");

            while (receivedMessage != "exit" && _isRunning)
            {
                try
                {
                    var (bytesRead, cmd) = await NetworkHelper.ReceiveIntDataAsync(ProtocolStandards.SizeMessageDefinedLength, stream);

                    if (bytesRead == 0)
                        break;

                    await optionHandler.HandleAsync(cmd);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    break;
                }
            }

            acceptedConnection.Close();
            lock (_activeConnections)
            {
                _activeConnections.Remove(acceptedConnection);
            }
        }
    }
}
