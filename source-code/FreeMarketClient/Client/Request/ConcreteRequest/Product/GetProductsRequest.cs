using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.Product;

public class GetProductsRequest : RequestTemplate
{
    internal override async Task ConcreteHandle(NetworkStream stream, string? userName)
    {
        Console.Clear();
        Console.WriteLine("Type Product Filer (Enter for no filter)");
        
        var filter = Console.ReadLine();
        filter = string.IsNullOrWhiteSpace(filter) ? "none" : filter;

        var messageLength = ByteHelper.ConvertStringToBytes(filter).Length;

        await SendLength(stream, messageLength);
        await SendData(stream, filter);

        await GetResponse(stream);

    }

    private async Task GetResponse(NetworkStream stream)
    {
        try
        {
            var (bytesRead, messageLength) =
                await NetworkHelper.ReceiveIntDataAsync(ProtocolStandards.SizeMessageDefinedLength, stream);

            if (bytesRead == 0 || messageLength == 0)
            {
                Console.Clear();
                Console.WriteLine("No products available");
                Thread.Sleep(1500);
                return;
            }

            (bytesRead, var productsString) = await NetworkHelper.ReceiveStringDataAsync(messageLength, stream);

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
        catch (NetworkHelper.ServerDisconnectedException ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(0);
        }
    }

}