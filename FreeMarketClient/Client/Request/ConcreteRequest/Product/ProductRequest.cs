using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.Product;

public abstract class ProductRequest : RequestTemplate
{
    internal ProductDTO? ProductDto;
    internal Socket Socket;

    protected abstract void HandleConcreteProductOperation();
    protected abstract void HandleImageSending();

    internal override void ConcreteHandle(Socket socket, string? userName)
    {
        Socket = socket;
        
        Console.Clear();
        Console.WriteLine("Type Product Name");
        var name = GetInputData();

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

        SendLength(Socket, messageLength);
        SendData(Socket, productData);

        HandleImageSending();

        GetServerResponse(Socket);
    }
    
}