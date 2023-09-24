using System.ComponentModel;
using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest;

public class ProductDeletionRequest : RequestTemplate
{
    internal override void ConcreteHandle(Socket socket)
    {
        Console.WriteLine("Type Product Name");
        var name = Console.ReadLine();

        var productDTO = new ProductDTO()
        {
            Name = name,
        };

        var productData = KOI.Stringify(productDTO);
        var messageLength = ByteHelper.ConvertStringToBytes(productData).Length;

        SendLength(socket, messageLength);
        SendData(socket, productData);
    }
}