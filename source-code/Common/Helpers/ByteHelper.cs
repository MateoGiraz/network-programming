using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class ByteHelper
    {
        public static byte[] ConvertStringToBytes(string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }

        public static byte[] ConvertIntToBytes(int length)
        {
            return BitConverter.GetBytes(length);
        }

        public static string ConvertBytesToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static int ConvertBytesToInt(byte[] data)
        {
            return BitConverter.ToInt32(data);
        }
    }
}
