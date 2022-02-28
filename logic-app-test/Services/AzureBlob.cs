using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using logic_app_test.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace logic_app_test.Services
{
    public class AzureBlob
    {
        private readonly BlobServiceClient _blobServiceClient;
        public AzureBlob(BlobServiceClient blobServiceClient) => _blobServiceClient = blobServiceClient;

        public Task AddFile(IFormFile file)
        {
            var blob = _blobServiceClient.GetBlobContainerClient(Settings.BlobContainerName);
            using var fileStream = file.OpenReadStream();
            var bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)file.Length);
            fileStream.Position = 0;
            blob.UploadBlob(Path.GetFileNameWithoutExtension(file.FileName), fileStream);
            return Task.CompletedTask;
        }

        public Task<byte[]> GetFile(string fileName)
        {
            var blob = _blobServiceClient.GetBlobContainerClient(Settings.BlobContainerName);
            using (var ms = new MemoryStream())
            {
                var file = blob.GetBlobClient(fileName);
                file.DownloadTo(ms);
                return Task.FromResult(ms.ToArray());
            }
        }

        public Task<Pageable<BlobItem>> GetAllFiles()
        {
            var container = _blobServiceClient.GetBlobContainerClient(Settings.BlobContainerName);
            var blobs = container.GetBlobs();
            return Task.FromResult(blobs);
        }
    }
}
