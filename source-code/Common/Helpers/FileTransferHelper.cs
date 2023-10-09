using Common.Protocol;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class FileTransferHelper
    {
        public async Task SendFileAsync(string filePath, NetworkStream stream)
        {
            try
            {
                var fileSize = await SendFileInfoAsync(filePath, stream);
                await SendFileDataAsync(filePath, fileSize, stream);

                Console.WriteLine("Finished sending file");
            }
            catch (SocketException ex)
            {
                HandleSocketException(ex);
                throw;
            }
        }

        public async Task<string> ReceiveFileAsync(NetworkStream stream)
        {
            try
            {
                var (fileName, fileSize) = await ReceiveFileInfoAsync(stream);
                var path = await ReceiveFileDataAsync(fileName, fileSize, stream);

                return path;
            }
            catch (SocketException ex)
            {
                HandleSocketException(ex);
                throw;
            }
        }

        private async Task<long> SendFileInfoAsync(string filePath, NetworkStream stream)
        {
            var fileName = Path.GetFileName(filePath);
            var fileNameBytes = Encoding.UTF8.GetBytes(fileName);
            var fileNameLengthBytes = BitConverter.GetBytes(fileNameBytes.Length);

            await NetworkHelper.SendMessageAsync(fileNameLengthBytes, stream);
            await NetworkHelper.SendMessageAsync(fileNameBytes, stream);

            var fileSize = new FileInfo(filePath).Length;
            var fileSizeBytes = BitConverter.GetBytes(fileSize);
            await NetworkHelper.SendMessageAsync(fileSizeBytes, stream);

            return fileSize;
        }

        private async Task<(string, int)> ReceiveFileInfoAsync(NetworkStream stream)
        {
            var (_, fileNameLength) = await NetworkHelper.ReceiveIntDataAsync(ProtocolStandards.SizeMessageDefinedLength, stream);
            var (_, fileName) = await NetworkHelper.ReceiveStringDataAsync(fileNameLength, stream);
            var (_, fileSize) = await NetworkHelper.ReceiveIntDataAsync(ProtocolStandards.FileDefinedLength, stream);

            return (fileName, fileSize);
        }

        private async Task SendFileDataAsync(string filePath, long fileSize, NetworkStream stream)
        {
            var fileStreamHelper = new FileStreamHelper();
            long offset = 0;
            long currentPart = 1;
            long totalParts = ProtocolStandards.CalculatePartCount(fileSize);

            while (offset < totalParts)
            {
                int partSize = currentPart == totalParts ? (int)(fileSize - offset) : ProtocolStandards.MaxPartSize;
                byte[] buffer = fileStreamHelper.Read(filePath, offset, partSize);
                await NetworkHelper.SendMessageAsync(buffer, stream);

                offset += partSize;
                currentPart++;
            }
        }

        private async Task<string> ReceiveFileDataAsync(string fileName, int fileSize, NetworkStream stream)
        {
            var projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            const string relativeDirectory = "Images";
            var directoryPath = Path.Combine(projectDirectory, relativeDirectory);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, fileName);
            var fileStreamHelper = new FileStreamHelper();

            long offset = 0;
            long currentPart = 1;
            long totalParts = ProtocolStandards.CalculatePartCount(fileSize);

            while (offset < totalParts)
            {
                int partSize = currentPart == totalParts ? (int)(fileSize - offset) : ProtocolStandards.MaxPartSize;
                var (_, buffer) = await NetworkHelper.ReceiveDataAsync(partSize, stream);
                fileStreamHelper.Write(filePath, buffer);

                offset += partSize;
                currentPart++;
            }

            return filePath;
        }

        private void HandleSocketException(SocketException ex)
        {
            Console.WriteLine($"Socket Exception: {ex.Message}");
        }
    }
}