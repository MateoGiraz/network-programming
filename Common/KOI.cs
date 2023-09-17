using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using CoreBusiness;


namespace Common;
public class KOI {
    public static string Stringify(object? obj) {

        var type = obj!.GetType();
        var props = type.GetProperties().ToList();
        var result = "";

        foreach (var prop in props)
        {
            var valor = prop.GetValue(obj);
            if (valor == null) continue;

            if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
            {
                result += prop.Name + "#" + valor.ToString() + "##";
                continue;
            }

            result += prop.Name + "#" + Stringify(valor);
        }

        return result[..^2];
    }

    public static List<Dictionary<string, string>> Parse(string str)
    {
        var result = new List<Dictionary<string, string>>();
        var objects = str.Split("###");
        
        foreach(var obj in objects)
        {
            var dic = new Dictionary<string, string>();
            var attributes = str.Split("##");
            
            foreach (var attribute in attributes)
            {
                var atr = attribute.Split("#");
                var key = atr[0];
                var value = atr[1];
                dic.Add(key,value); 
            }
            
            result.Add(dic);
        }
        
        return result;
    }

    public static void PrintEncoded(string encoded)
    {
        var dicList = Parse(encoded);
        Print(dicList);
    }

    public static void Print(List<Dictionary<string, string>> dicList)
    {
        foreach (var obj in dicList)
        {
            foreach (var pair in obj)
            {
                Console.WriteLine(pair);
            }
        }
    }
}