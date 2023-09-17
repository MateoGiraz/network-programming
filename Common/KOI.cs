using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using CoreBusiness;


namespace Common;
public class KOI {
    private const string SplitToken = "#";

    public static string Stringify(object? obj) {

        var type = obj!.GetType();
        var props = type.GetProperties().ToList();
        var result = type.Name + SplitToken;

        foreach (var prop in props)
        {
            var valor = prop.GetValue(obj);
            if (valor == null) continue;

            if (PropertyTypeIsPrimitive(prop))
            {
                result += prop.Name + SplitToken + valor + SplitToken + SplitToken;
                continue;
            }

            result += SplitToken + Stringify(valor);
        }

        return result[..^2];
    }

    public static Dictionary<string, Dictionary<string, string>> Parse(string str)
    {
        var result = new Dictionary<string, Dictionary<string, string>>();
        var objects = SplitObjects(str);
        
        foreach(var obj in objects)
        {
            var objData = GetObjectData(obj);
            var name = GetObjectName(objData);
            var attributes = GetObjectAttributes(objData);

            var dic = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                var atr = GetAttribute(attribute);
                var key = atr[0];
                var value = atr[1];
                dic.Add(key,value); 
            }
            
            result.Add(name, dic);
        }
        
        return result;
    }
    
    public static void PrintEncoded(string encoded)
    {
        var dicList = Parse(encoded);
        Print(dicList);
    }

    public static void Print(Dictionary<string, Dictionary<string, string>> dicPair)
    {
        foreach (var pair in dicPair)
        {
            Console.WriteLine(pair.Key);
            foreach (var dic in pair.Value)
            {
                Console.WriteLine(dic);
            }
        }
    }
    
    private static bool PropertyTypeIsPrimitive(PropertyInfo prop)
    {
        return prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string);
    }
    

    private static string[] GetAttribute(string attribute)
    {
        return attribute.Split(SplitToken);
    }

    private static string GetObjectName(string[] objData)
    {
        return objData[0];
    }

    private static string[] GetObjectAttributes(string[] objData)
    {
        return objData[1].Split(SplitToken + SplitToken);
    }

    private static string[] GetObjectData(string obj)
    {
        return obj.Split(SplitToken, 2);
    }

    private static string[] SplitObjects(string str)
    {
        return str.Split(SplitToken + SplitToken + SplitToken);
    }
}