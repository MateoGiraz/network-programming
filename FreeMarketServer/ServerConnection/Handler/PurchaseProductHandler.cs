using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using CoreBusiness;
using BusinessLogic;

namespace ServerConnection.Handler;

public class PurchaseProductHandler
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

        String productName = productMap["Name"].ToString();
        ProductController pc = new ProductController();
        Product purchasedProduct = pc.GetProduct(productName);
        pc.BuyProduct(purchasedProduct,1);
        
        

        Console.WriteLine("Received Product: " + productName);
        
    }
}