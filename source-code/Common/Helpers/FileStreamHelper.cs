using System;
using System.IO;

namespace Common.Helpers
{
    public class FileStreamHelper
    {
        public byte[] Read(string path, long offset, int length) 
        {
            var data = new byte[length];

            using (var fs = new FileStream(path, FileMode.Open))
            {
                fs.Position = offset;
                var bytesRead = 0;
                while (bytesRead < length)
                {
                    var read = fs.Read(data, bytesRead, length - bytesRead);
                    if (read == 0)
                    {
                        throw new Exception("Couldn't not read file");
                    }
                    bytesRead += read;
                }
            }

            return data;
        }

        public void Write(string filePath, byte[] data)
        {
            FileMode fileMode = File.Exists(filePath) ? FileMode.Append : FileMode.Create;

            using (var fs = new FileStream(filePath, fileMode))
            {
                fs.Write(data, 0, data.Length);
            }
        }
    }
}
