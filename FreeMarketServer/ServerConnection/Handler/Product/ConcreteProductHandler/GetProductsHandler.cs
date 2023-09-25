using BusinessLogic;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;
using System.Net.Sockets;

namespace ServerConnection.Handler.Product.ConcreteProductHandler;

public class GetProductsHandler
{
    public void Handle(Socket socket)
    {
        
        var pc = new ProductController();
        var productsDto = pc.GetProducts().Select(product => new ProductDTO()
        {
            Name = product.Name,
            Description = product.Description,
            Stock = product.Stock.ToString(),
            Price = product.Price.ToString(),
            
        }).ToList();;

        var listNameDto = new ProductNameListDto()
        {
            Filter = "none",
            ProductNames = productsDto
        };

        foreach (var prod in listNameDto.ProductNames)
        {
            Console.WriteLine(prod.Name);
        }
            
        var productsData = KOI.Stringify(listNameDto);
        Console.WriteLine(productsData);
        var messageLength = ByteHelper.ConvertStringToBytes(productsData).Length;
        
        NetworkHelper.SendMessage(ByteHelper.ConvertIntToBytes(messageLength), socket);
        NetworkHelper.SendMessage(ByteHelper.ConvertStringToBytes(productsData), socket);
    }
}