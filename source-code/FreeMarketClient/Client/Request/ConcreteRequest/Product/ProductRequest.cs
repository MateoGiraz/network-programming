using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.Product;

public abstract class ProductRequest : RequestTemplate
{
    internal ProductDTO? ProductDto;
    internal NetworkStream Stream;

    protected abstract Task HandleConcreteProductOperationAsync();
    protected abstract Task HandleImageSendingAsync();

    internal override async Task ConcreteHandleAsync(NetworkStream stream, string? userName)
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

        await HandleConcreteProductOperationAsync();

        var productData = KOI.Stringify(ProductDto);
        var messageLength = ByteHelper.ConvertStringToBytes(productData).Length;

        await SendLengthAsync(Stream, messageLength);
        await SendDataAsync(Stream, productData);

        await HandleImageSendingAsync();

        await GetServerResponseAsync(Stream);
    }
    
}