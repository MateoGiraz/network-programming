using System.Net.Sockets;
using BusinessLogic;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using MemoryRepository;

namespace ServerConnection.Handler.Product.GetProducts;

public class GetProductHandler
{
    public async Task HandleAsync(NetworkStream stream)
    {
        var (bytesRead, messageLength) = await
            NetworkHelper.ReceiveIntDataAsync(ProtocolStandards.SizeMessageDefinedLength, stream);

        if (bytesRead == 0)
            return;

        (bytesRead, var getRequest) = await NetworkHelper.ReceiveStringDataAsync(messageLength, stream);

        if (bytesRead == 0)
            return;

        SendResponse(stream, getRequest);
    }

    private static void SendResponse(NetworkStream stream, string getRequest)
    {
        var requestMap = KOI.Parse(getRequest);

        var name = requestMap["Name"].ToString();
        var getImage = requestMap["GetImage"].ToString();
        
        var pc = new ProductController();
        var productDto = new ProductDTO();
        
        try
        {
            var product = pc.GetProduct(name);
            
            productDto.Name = product.Name;
            productDto.Description = product.Description;
            productDto.ImageRoute = product.ImageRoute;
            productDto.Stock = product.Stock.ToString();
            productDto.Price = product.Price.ToString();
            productDto.ImageRoute = product.ImageRoute;
            productDto.Ratings = product.Ratings.Select(r =>
                new RatingDTO()
                    {
                        Comment = r.Comment,
                        Score = r.Score.ToString()
                    })
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ex: " + ex.Message);
        }


        var productsData = KOI.Stringify(productDto);
        var messageLength = ByteHelper.ConvertStringToBytes(productsData).Length;

        NetworkHelper.SendMessageAsync(ByteHelper.ConvertIntToBytes(messageLength), stream);
        NetworkHelper.SendMessageAsync(ByteHelper.ConvertStringToBytes(productsData), stream);
        
        
        var shouldSendImage = getImage.ToLower().Equals("y");

        if (shouldSendImage)
        {
            try
            {
                var fileTransferHelper = new FileTransferHelper();
                fileTransferHelper.SendFileAsync(productDto.ImageRoute, stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

    }
}