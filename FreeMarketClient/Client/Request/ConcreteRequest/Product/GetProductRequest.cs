using System.Net.Sockets;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.Product;

public class GetProductRequest : RequestTemplate
{
    internal override void ConcreteHandle(Socket socket, string? userName)
    {
        Console.Clear();
        Console.WriteLine("Type Product Name");

        var name = GetInputData();

        var messageLength = ByteHelper.ConvertStringToBytes(name).Length;

        SendLength(socket, messageLength);
        SendData(socket, name);

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

        var prod = KOI.Parse(productString);
        
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
      
        Console.WriteLine("Enter key to go back...");
        Console.ReadLine();

    }
}