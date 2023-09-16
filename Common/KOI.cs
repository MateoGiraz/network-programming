using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using CoreBusiness;


namespace Common;
    public class KOI
    {
        public static string stringify(object? obj)
        {
            string result = "";
            List<PropertyInfo> listaPropiedades = ObtenerPropiedades(obj);
            int i = 0;
            Console.WriteLine("Entro");
            foreach (PropertyInfo propiedad in listaPropiedades)
            {
                i++;
                result +=(propiedad.Name+"#"+ propiedad.GetValue(obj).ToString());
                if (listaPropiedades.Count() != i)
                {
                    result += "##";
                }
            }
            return result;
        }

        static List<PropertyInfo> ObtenerPropiedades(object obj)
        {
            Type tipo = obj.GetType();
            List<PropertyInfo> propiedades = tipo.GetProperties().ToList();
            return propiedades;
        }

        public static Dictionary<string, string> Parse(string str)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string[] Atributes = str.Split("##");
            string[] Pieces = new string[2];
            for (int i = 0; i < Atributes.Length; i++) {
                Pieces = Atributes[i].Split("#");
                result.Add(Pieces[0],Pieces[1]);
            }

            return result;
        }
}