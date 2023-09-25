using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace ServerConnection.Handler.Product;

public abstract class ProductHandler
{
    internal ProductDTO? ProductDto;
    internal ResponseDTO? ResponseDto;
    internal UserDTO? UserDto;
    internal Dictionary<string, object>? ProductMap;
    
    protected abstract void HandleProductSpecificOperation();

    internal void Handle(Socket socket)
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var productString) = NetworkHelper.ReceiveStringData(messageLength, socket);

        if (bytesRead == 0)
            return;
        
        Console.WriteLine(productString);
        ProductMap = KOI.Parse(productString);
        var userMap = KOI.GetObjectMap(ProductMap["Owner"]);

        UserDto = new UserDTO()
        {
            UserName = userMap["UserName"]
        };
        
        ProductDto = new ProductDTO()
        {
            Name = ProductMap["Name"] as string,
            Owner = UserDto
        };

        ResponseDto = new ResponseDTO();
        
        try
        {
            HandleProductSpecificOperation();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            ResponseDto.StatusCode = 400;
            ResponseDto.Message = ex.Message;
        }
        
        var responseData = KOI.Stringify(ResponseDto);
        var responseMessageLength = ByteHelper.ConvertStringToBytes(responseData).Length;

        NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(responseMessageLength), socket);
        NetworkHelper.SendMessage(ByteHelper.ConvertStringToBytes(responseData), socket);

    }
}