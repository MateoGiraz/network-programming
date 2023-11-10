using protos.sale;
using Grpc.Net.Client;
using Common.Config;

using Product = CoreBusiness.Product;
using System.Net;

namespace ServerConnection.gRPC;

public class GrpcProvider
{
    private GrpcChannel? channel;
    private SaleService.SaleServiceClient? client;
    
    public GrpcProvider()
    {
        ISettingsManager settingsManager = new SettingsManager();

        var saleGrpcIpAddress = settingsManager.Get(ServerConfig.SaleGrpcIpAddress);
        var saleGrpcPort = settingsManager.Get(ServerConfig.SaleGrpcPort);

        channel = GrpcChannel.ForAddress($"http://{saleGrpcIpAddress}:{saleGrpcPort}");
        client = new SaleService.SaleServiceClient(channel);
    }

    public async Task<(bool, string)> CreateSaleAsync(Product product, string username)
    {
        try
        {
            var protoProduct = new global::protos.sale.Product()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
                
            var reply = await client.CreateSaleAsync(new Sale
            {
                Product = protoProduct,
                Username = username
            });

            return (false, reply.Result);
        }
        catch (Exception e)
        {
            return (true, e.Message);
        }
    }
}
