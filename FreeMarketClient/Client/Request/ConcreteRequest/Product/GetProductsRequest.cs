using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.Product;

public class GetProductsRequest : RequestTemplate
{
    internal ProductNameListDTO _productNameListDto;
    internal Dictionary<string, object> listOfNamesMap;

    internal override void ConcreteHandle(Socket socket, string? userName)
    {
        Console.Clear();
        Console.WriteLine("This is the List of product: ");
        
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var productsString) = NetworkHelper.ReceiveStringData(messageLength, socket);

        if (bytesRead == 0)
            return;

        listOfNamesMap = KOI.Parse(productsString);

        var names = KOI.GetObjectMapList(listOfNamesMap!["productsNames"]);

        for (int i = 0; i < names.Count; i++)
        {
            Console.WriteLine(i + ". " + names[i]);
        }
    }

}