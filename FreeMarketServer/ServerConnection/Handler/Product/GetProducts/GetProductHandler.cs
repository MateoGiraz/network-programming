using System.Net.Sockets;
using BusinessLogic;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using MemoryRepository;

namespace ServerConnection.Handler.Product.GetProducts;

public class GetProductHandler
{
    public void Handle(Socket socket)
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var getRequest) = NetworkHelper.ReceiveStringData(messageLength, socket);

        if (bytesRead == 0)
            return;

        SendResponse(socket, getRequest);
    }

    private static void SendResponse(Socket socket, string getRequest)
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

        NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(messageLength), socket);
        NetworkHelper.SendMessage(ByteHelper.ConvertStringToBytes(productsData), socket);
        
        
        var shouldSendImage = getImage.ToLower().Equals("y");

        if (shouldSendImage)
        {
            try
            {
                var fileTransferHelper = new FileTransferHelper();
                fileTransferHelper.SendFile(socket, productDto.ImageRoute);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

    }
}