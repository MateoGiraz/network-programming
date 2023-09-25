using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest.Product;

public abstract class ProductRequest : RequestTemplate
{
    internal ProductDTO? ProductDto;
    internal ResponseDTO? ResponseDto;

    protected abstract void HandleConcreteProductOperation();
    internal override void ConcreteHandle(Socket socket, string? userName)
    {
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

        SendLength(socket, messageLength);
        SendData(socket, productData);

        GetServerResponse(socket);
    }
    
    private void GetServerResponse(Socket socket)
    {
        var (bytesRead, responseLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var responseString) = NetworkHelper.ReceiveStringData(responseLength, socket);

        if (bytesRead == 0)
            return;

        var responseMap = KOI.Parse(responseString);
        var statusCodeValue = responseMap["StatusCode"] as string;
        var messageValue = responseMap["Message"] as string;
        
        ResponseDto = new ResponseDTO()
        {
            StatusCode = int.Parse(statusCodeValue ?? "500"),
            Message = messageValue ?? "Internal Server Error"
        };

        Console.Clear();
        Console.WriteLine(ResponseDto.Message);
        Thread.Sleep(1500);
    }
}