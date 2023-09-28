using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.Product;

public class GetProductRequest : RequestTemplate
{
    private bool _getImage = false;
    internal override void ConcreteHandle(Socket socket, string? userName)
    {
        Console.Clear();
        var name = InputHelper.GetValidInput("Type Product Name");

        
        Console.WriteLine($"Download {name}'s image? (Y/N)");
        var response = GetInputData();

        _getImage = response.ToLower().Equals("y");

        var productGetRequest = new ProductGetRequest()
        {
            Name = name,
            GetImage = _getImage ? "y" : "n",
        };

        var request = KOI.Stringify(productGetRequest);
        
        var messageLength = ByteHelper.ConvertStringToBytes(request).Length;

        SendLength(socket, messageLength);
        SendData(socket, request);

        GetResponse(socket);
    }

    private void GetResponse(Socket socket)
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var productString) = NetworkHelper.ReceiveStringData(messageLength, socket);

        if (bytesRead == 0)
            return;

        Dictionary<string, object> prod;
        
        try
        {
            prod = KOI.Parse(productString);
        }
        catch (Exception e)
        {
            Console.WriteLine("Product was not found");
            Thread.Sleep(1500);
            return;
        }
        
        var ratings = prod.TryGetValue("Ratings", out var value) ? KOI.GetObjectMapList(value) : null;

        Console.Clear();
        Console.WriteLine($"{prod["Name"]}.");
        Console.WriteLine($"{prod["Stock"]} units left for ${prod["Price"]}");
        Console.WriteLine($"{prod["Description"]}.");

        if (ratings != null)
        {
            Console.WriteLine("Ratings");
            var index = 0;
            for (; index < ratings.Count; index++)
            {
                var rating = ratings[index];
                Console.WriteLine($"{index + 1}. Comment: {rating["Comment"]}, Score: {rating["Score"]}.");
            }
        }

        if (_getImage)
        {
            var fileTransferHelper = new FileTransferHelper();
            var path = fileTransferHelper.ReceiveFile(socket);
            
            Console.WriteLine($"Downloaded image at path: {path}");
        }

        Console.WriteLine("Enter key to go back...");
        Console.ReadLine();

    }
}