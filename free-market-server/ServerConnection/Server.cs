using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ServerConnection;

public class Server
{
    public void Listen(int port = 3000)
    {
        var serverSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
        );

        var localEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        serverSocket.Bind(localEndpoint);

        Console.WriteLine("Listening for connections");
        serverSocket.Listen(100);

        while (true)
        {
            var acceptedConnection = serverSocket.Accept();
            Console.WriteLine($"Connected to client: {acceptedConnection.RemoteEndPoint}");

            try
            {
                byte[] receivedData = new byte[1024];
                int bytesRead = acceptedConnection.Receive(receivedData);

                if (bytesRead > 0)
                {
                    //TODO: IMPLEMENTAR NUESTRO PROPIO PROTOCOLO. NO PODEMOS USAR JSON.
                    string jsonString = Encoding.UTF8.GetString(receivedData, 0, bytesRead);
                    Console.WriteLine($"Received data from {acceptedConnection.RemoteEndPoint} is {jsonString}");

                    var credentials = JsonSerializer.Deserialize<UserCredentials>(jsonString);

                    if (IsValidUser(credentials))
                    {
                        byte[] sendData = Encoding.Default.GetBytes(
                            $"Valid credentials received from {acceptedConnection.RemoteEndPoint}: Username: {credentials.Username}, Password: {credentials.Password}");
                        acceptedConnection.Send(sendData);
                    }
                    else
                    {
                        byte[] sendData = Encoding.Default.GetBytes(
                            $"Invalid credentials received from {acceptedConnection.RemoteEndPoint}: Username: {credentials.Username}, Password: {credentials.Password}");
                        acceptedConnection.Send(sendData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }

    // TODO: CREAR UN PAQUETE AUTENTICADOR, NADA DE ESTO TIENE QUE ESTAR ACA
    private class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // TODO: ES UN EJEMPLO, EL SERVIDOR DEBERIA HABLAR CON UN CONTROLADOR DE FACHADA PARA TODO ESTO
    private bool IsValidUser(UserCredentials credentials)
    {
        return credentials.Username == "admin" && credentials.Password == "password";
    }
}