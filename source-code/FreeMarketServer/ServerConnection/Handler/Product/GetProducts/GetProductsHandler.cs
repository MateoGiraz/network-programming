using BusinessLogic;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using System.Net.Sockets;

namespace ServerConnection.Handler.Product.GetProducts;

public class GetProductsHandler
{
    public async Task HandleAsync(NetworkStream stream)
    {
        var (bytesRead, messageLength) = await
            NetworkHelper.ReceiveIntDataAsync(ProtocolStandards.SizeMessageDefinedLength, stream);

        if (bytesRead == 0)
            return;

        (bytesRead, var filter) = await NetworkHelper.ReceiveStringDataAsync(messageLength, stream);

        if (bytesRead == 0)
            return;
        
        SendResponse(stream, filter);
    }

    private static void SendResponse(NetworkStream stream, string filter)
    {
        var pc = new ProductController();
        
        var productsDto =  pc.GetProducts()
            .Where(p => p.Name.ToLower().Contains(filter.ToLower()) || filter == "none")
            .Select(product => 
                new ProductDTO(){
                    Name = product.Name,
                    Description = product.Description,
                    Stock = product.Stock.ToString(),
                    Price = product.Price.ToString(),
                })
            .ToList();

        var listNameDto = new ProductNameListDto()
        {
            Filter = filter,
            ProductNames = productsDto
        };

        try
        {
            var productsData = KOI.Stringify(listNameDto);
            var messageLength = ByteHelper.ConvertStringToBytes(productsData).Length;

            NetworkHelper.SendMessageAsync(ByteHelper.ConvertIntToBytes(messageLength), stream);
            NetworkHelper.SendMessageAsync(ByteHelper.ConvertStringToBytes(productsData), stream);
        }
        catch (ArgumentOutOfRangeException e)
        {
            NetworkHelper.SendMessageAsync(ByteHelper.ConvertIntToBytes(0), stream);
        }

    }
}