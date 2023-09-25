using BusinessLogic;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using System.Net.Sockets;

namespace ServerConnection.Handler.Product.ConcreteProductHandler;

public class GetProductsHandler
{
    public ProductNameListDTO listNameDTO;
    
    public void Handle(Socket socket)
    {
        
        var pc = new ProductController();
        listNameDTO.productsNames = pc.GetProductsNames();
        
        var productsData = KOI.Stringify(listNameDTO);
        var messageLength = ByteHelper.ConvertStringToBytes(productsData).Length;
        
        NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(messageLength), socket);
        NetworkHelper.SendMessage(ByteHelper.ConvertStringToBytes(productsData), socket);

    }
}