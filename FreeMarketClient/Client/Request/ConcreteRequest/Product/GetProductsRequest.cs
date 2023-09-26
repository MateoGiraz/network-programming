using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.Product;

public class GetProductsRequest : RequestTemplate
{
    internal override void ConcreteHandle(Socket socket, string? userName)
    {
        Console.Clear();
        Console.WriteLine("Type Product Filer (Enter for no filter)");
        
        var filter = Console.ReadLine();
        filter = string.IsNullOrWhiteSpace(filter) ? "none" : filter;

        var messageLength = ByteHelper.ConvertStringToBytes(filter).Length;

        SendLength(socket, messageLength);
        SendData(socket, filter);
        
        GetResponse(socket);
    }

    private void GetResponse(Socket socket)
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var productsString) = NetworkHelper.ReceiveStringData(messageLength, socket);

        if (bytesRead == 0)
            return;

        var listOfNamesMap = KOI.Parse(productsString);
        var names = KOI.GetObjectMapList(listOfNamesMap["ProductNames"]);
        
        Console.Clear();
        Console.WriteLine("System products: ");

        var index = 0;
        for (; index < names.Count; index++)
        {
            var prod = names[index];
            Console.WriteLine($"{index + 1}. {prod["Name"]}: {prod["Stock"]} units left for ${prod["Price"]}.");
        }

        Console.WriteLine("Enter key to go back...");
        Console.ReadLine();
    }

}