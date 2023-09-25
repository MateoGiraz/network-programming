using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;


namespace Common.Protocol;
public class KOI {
    private const string SplitToken = "#";
    private const string ListSuffix = "_List";
    private const string UnderScore = "_";

    public static string Stringify(object? obj, string name = "") {
         
        var type = obj!.GetType();
        var props = type.GetProperties().ToList();

        if (name == "")
            name = type.Name;
            
        var result = name + SplitToken;

        foreach (var prop in props)
        {
            var val = prop.GetValue(obj);
            if (val == null) continue;

            if (PropertyTypeIsPrimitive(prop))
            {
                result += prop.Name + SplitToken + val + SplitToken + SplitToken;
                continue;
            }

            if (val is IList list)
            {
                result += SplitToken + prop.Name + UnderScore + GetListTypeName(list) + ListSuffix + SplitToken;
                foreach (var listItem in list)
                {
                    result += Stringify(listItem) + SplitToken + SplitToken;
                }
                continue;
            }

            result += SplitToken + Stringify(val, prop.Name) + SplitToken + SplitToken;
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

            if (ItemIsList(name))
            {
                HandleListItem(name, objData, result);
                continue;
            }
            
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

    private static void HandleListItem(string name, string[] objData, Dictionary<string, object> result)
    {
        var retList = new List<Dictionary<string, string>>();

        var splitName = name.Split(UnderScore, 3);
        var listTypeName = splitName[1];
        name = splitName[0];

        char[] removeFromListAttributes = { '#', ' ' };
        var attributes = GetListObjectAttributes(objData, listTypeName);
        attributes = attributes.Select(listAttr => listAttr.Trim(removeFromListAttributes)).ToArray();

        foreach (var listAttr in attributes)
        {
            var listDic = new Dictionary<string, string>();
            foreach (var pair in listAttr.Split(SplitToken + SplitToken))
            {
                var (key, value) = GetAttribute(pair);
                listDic.Add(key, value);
            }

            retList.Add(listDic);
        }

        result.Add(name, retList);
    }

    public static Dictionary<string, string> GetObjectMap(object obj)
    {
        if (obj is Dictionary<string, string> objDic)
        {
            return objDic;
        }

        throw new Exception("Obj is not a string -> string map");
    }

    public static List<Dictionary<string, string>> GetObjectMapList(object obj)
    {
        var ret = new List<Dictionary<string, string>>();
        
        if (obj is IList list)
        {
            foreach (var item in list)
            {
                var itemMap = KOI.GetObjectMap(item);
                ret.Add(itemMap);
            }

            return ret;
        }
        throw new Exception("Obj is not a List of string -> string map");
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
    
    private static bool ItemIsList(string name)
    {
        var toCheckName = name.Split(UnderScore, 3);
        return toCheckName.Length > 1;
    }
    
    private static string[] GetListObjectAttributes(string[] objData, string listTypeName)
    {
        return objData[1].Split(listTypeName).Skip(1).ToArray();
    }
    
    private static string GetListTypeName(IList list)
    {
        return list[0]!.GetType().Name;
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