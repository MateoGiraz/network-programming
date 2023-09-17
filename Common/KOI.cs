using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
//using CoreBusiness;


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
    
    public static Dictionary<string, object> Parse(string str)
    {
        var result = new Dictionary<string, object>();
        var objects = SplitObjects(str);
        var isFirstObject = true;
        
        foreach(var obj in objects)
        {
            var objData = GetObjectData(obj);
            var name = GetObjectName(objData);
            var attributes = GetObjectAttributes(objData);

            var dic = new Dictionary<string, string>();
            foreach (var attribute in attributes)
            {
                var (key, value) = GetAttribute(attribute);

                if (isFirstObject)
                {
                    result.Add(key, value);
                    continue;
                } 
                
                dic.Add(key, value);
            }


            if (!isFirstObject)
            {
                result.Add(name, dic);
            }
            
            isFirstObject = false;
        }
        
        return result;
    }

    public static Dictionary<string, string> GetObjectMap(object obj)
    {
        if (obj is Dictionary<string, string> objDic)
        {
            return objDic;
        }

        throw new Exception("Obj is not a string -> string map");
    }
    
    public static void PrintEncoded(string encoded)
    {
        var dicList = Parse(encoded);
        Print(dicList);
    }

    public static void Print(Dictionary<string, object> dicPair)
    {
        foreach (var (key, value) in dicPair)
        {
            Console.WriteLine(key);
            if (value is string)
            {
                Console.WriteLine(value);
                continue;
            }

            if (value is not Dictionary<string, string> map) 
                continue;
            
            foreach (var mapPair in map)
            {
                Console.WriteLine(mapPair);
            }


        }
    }
    
    private static bool PropertyTypeIsPrimitive(PropertyInfo prop)
    {
        return prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string);
    }
    

    private static (string, string) GetAttribute(string attribute)
    {
        var data = attribute.Split(SplitToken);
        return (data[0], data[1]);
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