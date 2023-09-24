﻿using System.ComponentModel;
using System.Net.Sockets;
using Common.DTO;
using Common.Helpers;
using Common.Protocol;

namespace free_market_client.Request.ConcreteRequest;

public class ProductCreationRequest : RequestTemplate
{
    internal override void ConcreteHandle(Socket socket)
    {
        Console.WriteLine("Type Product Name");
        var name = Console.ReadLine();
        
        Console.WriteLine($"Add {name}'s Description");
        var description = Console.ReadLine();

        Console.WriteLine("Type Price");
        var price = Console.ReadLine();
        
        Console.WriteLine("Type Stock");
        var stock = Console.ReadLine();

        var productDTO = new ProductDTO()
        {
            Name = name,
            Description = description,
            Price = price,
            Stock = stock,
            
        };

        var productData = KOI.Stringify(productDTO);
        var messageLength = ByteHelper.ConvertStringToBytes(productData).Length;

        SendLength(socket, messageLength);
        SendData(socket, productData);
    }
}