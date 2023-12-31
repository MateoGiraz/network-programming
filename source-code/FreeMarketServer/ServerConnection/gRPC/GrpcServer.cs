using System.Net;
using Common.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using protos.product;

namespace ServerConnection.gRPC.protos;

public class GrpcServer
{
    public void Listen()
    {
        ISettingsManager settingsManager = new SettingsManager();

        var serverGrpcPortConfigKey = int.Parse(settingsManager.Get(ServerConfig.ServerGrpcPortConfigKey));

        Console.WriteLine("Listening as gRPC server...");
        
        var builder = WebApplication.CreateBuilder();

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, serverGrpcPortConfigKey, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        });
        
        builder.Services.AddGrpc();
        var app = builder.Build();

        app.MapGrpcService<GrpcService>();
        app.MapGet("/", () => "Communication with gRPC server must be done via gRPC client 😑");
        
        app.Run();
    }
}