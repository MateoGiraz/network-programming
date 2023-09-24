using System.ComponentModel;
using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest;

public class ProductRatingRequest : RequestTemplate
{
    internal override void ConcreteHandle(Socket socket)
    {
        Console.WriteLine("Type Product Name");
        var name = Console.ReadLine();

        Console.WriteLine("Add Rating Score");
        var score = Console.ReadLine();
        
        Console.WriteLine("Add Rating Comment");
        var comment = Console.ReadLine();

        var ratingDto = new RatingDTO()
        {
            Comment = comment,
            Score = score,
        };
        
        var ratings = new List<RatingDTO>()
        {
            ratingDto
        };

        var productDTO = new ProductDTO()
        {
            Name = name,
            Ratings = ratings
        };

        var productData = KOI.Stringify(productDTO);
        var messageLength = ByteHelper.ConvertStringToBytes(productData).Length;

        SendLength(socket, messageLength);
        SendData(socket, productData);
    }
}