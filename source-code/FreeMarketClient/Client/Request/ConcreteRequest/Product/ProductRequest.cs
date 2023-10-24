using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.Product;

public abstract class ProductRequest : RequestTemplate
{
    internal ProductDTO? ProductDto;
    internal NetworkStream Stream;

    protected abstract void HandleConcreteProductOperation();
    protected abstract void HandleImageSending();

    internal override void ConcreteHandle(NetworkStream stream, string? userName)
    {
        Stream = stream;
        
        Console.Clear();
        var name = InputHelper.GetValidInput("Type Product Name");

        var userDto = new UserDTO()
        {
            UserName = userName!
        };
        
        ProductDto = new ProductDTO()
        {
            Name = name,
            Owner = userDto
        };
        
        HandleConcreteProductOperation();

        var productData = KOI.Stringify(ProductDto);
        var messageLength = ByteHelper.ConvertStringToBytes(productData).Length;

        SendLength(Stream, messageLength);
        SendData(Stream, productData);

        HandleImageSending();

        GetServerResponse(Stream);
    }
    
}