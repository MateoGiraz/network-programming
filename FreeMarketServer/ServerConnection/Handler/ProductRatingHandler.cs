using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using CoreBusiness;
using BusinessLogic;

namespace ServerConnection.Handler;

public class PurchaseRatingHandler
{
    //Quizás puede ser genérico
    internal static void Handle(Socket socket)
    {
        var (bytesRead, messageLength) =
            NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);

        if (bytesRead == 0)
            return;

        (bytesRead, var productString) = NetworkHelper.ReceiveStringData(messageLength, socket);

        if (bytesRead == 0)
            return;

        var productMap = KOI.Parse(productString);

        var productName = productMap["Name"].ToString();
        var ratingList = KOI.GetObjectMapList(productMap["Ratings"]);
        var newRating = ratingList[0];
        
        var pc = new ProductController();
        var toUpdateProduct = pc.GetProduct(productName);

        var updatedProduct = new Product()
        {
            Name = toUpdateProduct.Name,
            Description = toUpdateProduct.Description,
            Owner = toUpdateProduct.Owner,
            Price = toUpdateProduct.Price,
            ImageRoute = toUpdateProduct.ImageRoute,
            Ratings = toUpdateProduct.Ratings,
        };

        var score = int.Parse(newRating["Score"]);
        var comment = newRating["Comment"];
        
        updatedProduct.AddRating(score, comment);
        
        pc.UpdateProduct(toUpdateProduct.Name, toUpdateProduct.Owner, updatedProduct);
        Console.WriteLine("Product Rating: " + updatedProduct.Ratings);
    }
}