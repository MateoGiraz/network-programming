using protos.sale;
using Grpc.Net.Client;
using Product = CoreBusiness.Product;

namespace ServerConnection.gRPC;

public class GrpcProvider
{
    private GrpcChannel? channel;
    private SaleService.SaleServiceClient? client;
    
    public GrpcProvider()
    {
        channel = GrpcChannel.ForAddress("http://localhost:50001");
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
