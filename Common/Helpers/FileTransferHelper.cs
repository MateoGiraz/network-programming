using Common.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Common.Helpers
{
    public class FileTransferHelper
    {
        public void SendFile(Socket socket, string filePath)
        {
            try
            {
                var fileSize = SendFileInfo(socket, filePath);
                SendFileData(socket, filePath, fileSize);

                Console.WriteLine("Finished sending file");
            }
            catch (SocketException ex)
            {
                HandleSocketException(ex);
                throw;
            }
        }

        public string ReceiveFile(Socket socket)
        {
            try
            {
                var (fileName, fileSize) = ReceiveFileInfo(socket);
                var path = ReceiveFileData(socket, fileName, fileSize);
                
                return path;
            }
            catch (SocketException ex)
            {
                HandleSocketException(ex);
                throw;
            }
        }

        private long SendFileInfo(Socket socket, string filePath)
        {
            //elegir donde se guardan xd
            string myPicturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string fileName = Path.GetFileName(filePath);
            
            filePath = Path.Combine(myPicturesPath, fileName);
            
            byte[] fileNameBytes = ByteHelper.ConvertStringToBytes(fileName);

            int fileNameLength = fileNameBytes.Length;
            byte[] fileNameLengthBytes = BitConverter.GetBytes(fileNameLength);

            NetworkHelper.SendMessage(fileNameLengthBytes, socket);
            NetworkHelper.SendMessage(fileNameBytes, socket);

            long fileSize = new FileInfo(filePath).Length;
            byte[] fileSizeBytes = BitConverter.GetBytes(fileSize);
            NetworkHelper.SendMessage(fileSizeBytes, socket);

            return fileSize;
        }

        private (string, int) ReceiveFileInfo(Socket socket)
        {

            var (_, fileNameLength) = NetworkHelper.ReceiveIntData(ProtocolStandards.SizeMessageDefinedLength, socket);
            var (_, fileName) = NetworkHelper.ReceiveStringData(fileNameLength, socket);
            var (_, fileSize) = NetworkHelper.ReceiveIntData(ProtocolStandards.FileDefinedLength, socket);
            
            return (fileName, fileSize);
        }

        private void SendFileData(Socket socket, string filePath, long fileSize)
        {
            var fileStreamHelper = new FileStreamHelper();
            long offset = 0;
            long currentPart = 1;
            long totalParts = ProtocolStandards.CalculatePartCount(fileSize);

            while (offset < totalParts)
            {
                int partSize = currentPart == totalParts ? (int) (fileSize - offset) : ProtocolStandards.MaxPartSize;
                byte[] buffer = fileStreamHelper.Read(filePath, offset, partSize);
                NetworkHelper.SendMessage(buffer, socket);

                offset += partSize;
                currentPart++;
            }
        }

        private string ReceiveFileData(Socket socket, string fileName, int fileSize)
        {
            const string relativeFolderPath = "Images";
            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeFolderPath);
            var filePath = Path.Combine(folderPath, fileName);

            var fileStreamHelper = new FileStreamHelper();
            long offset = 0;
            long currentPart = 1;
            long totalParts = ProtocolStandards.CalculatePartCount(fileSize);

            while (offset < totalParts)
            {
                int partSize = currentPart == totalParts ? (int) (fileSize - offset) : ProtocolStandards.MaxPartSize;
                var (_, buffer) = NetworkHelper.ReceiveData(partSize, socket);

                fileStreamHelper.Write(filePath, buffer);

                offset += partSize;
                currentPart++;
            }

            return filePath;
        }

        private void HandleSocketException(SocketException ex)
        {
            Console.WriteLine($"Socket Exception: {ex.Message}, Error Code: {ex.ErrorCode}");
        }
    }

}
