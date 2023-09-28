using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common;
using Common.Config;
using Common.Protocol;
using Common.Helpers;

namespace ServerConnection
{
    public class Server
    {
        private readonly List<Socket> _activeConnections = new List<Socket>();
        private Socket _serverSocket;
        private bool _isRunning = true;

        public void Listen(int port = ProtocolStandards.ServerPort)
        {
            _serverSocket = SocketManager.Create(port);

            while (_isRunning)
            {
                try
                {
                    var acceptedConnection = _serverSocket.Accept();
                    lock (_activeConnections)
                    {
                        _activeConnections.Add(acceptedConnection);
                    }
                    new Thread(() => HandleConnection(acceptedConnection)).Start();
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
            
            _serverSocket?.Close();
        }

        private void HandleConnection(Socket acceptedConnection)
        {
            var optionHandler = new OptionHandler(acceptedConnection);
            var receivedMessage = "";
            Console.WriteLine($"Connected to client: {acceptedConnection.RemoteEndPoint}");

            while (receivedMessage is not "exit" && _isRunning)
            {

                try
                {
                    var (bytesRead, cmd) = NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, acceptedConnection);

                    if (bytesRead == 0)
                        break;

                    optionHandler.Handle(cmd);
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

    
