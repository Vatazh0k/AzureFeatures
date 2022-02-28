using Azure.Storage.Blobs;
using logic_app_test.AzureModel;
using logic_app_test.Infrastructure;
using logic_app_test.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace logic_app_test.Services
{
    public class AzureStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly CloudTableClient _cloudTableClient;

        public AzureStorageService(
            BlobServiceClient _blobServiceClient,
            CloudTableClient _cloudTableClient)
        {
            this._blobServiceClient = _blobServiceClient;
            this._cloudTableClient = _cloudTableClient;
        }

        public Task AddFileToBlob(IFormFile file)
        {
            var blob = _blobServiceClient.GetBlobContainerClient(Settings.BlobContainerName);
            using var fileStream = file.OpenReadStream();
            var bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)file.Length);
            fileStream.Position = 0;
            blob.UploadBlob(Path.GetFileNameWithoutExtension(file.FileName), fileStream);
            return Task.CompletedTask;
        }

        public Task<byte[]> ReadFileFromBlob(string fileName)
        {
            var blob = _blobServiceClient.GetBlobContainerClient(Settings.BlobContainerName);
            using (var ms = new MemoryStream())
            {
                var file = blob.GetBlobClient(fileName);
                file.DownloadTo(ms);
                return Task.FromResult(ms.ToArray());
            }
        }

        public Task AddFileDescriptionToTable(string fileDescription, string fileName)
        {
            var tableName = Path.GetFileNameWithoutExtension(fileName.Replace("_", ""));
            var table = _cloudTableClient.GetTableReference(tableName);
            table.CreateIfNotExistsAsync().Wait();
            var insertOperation = TableOperation.Insert(new FileMeta(Path.GetFileNameWithoutExtension(fileName), fileDescription));
            table.ExecuteAsync(insertOperation);
            return Task.CompletedTask;
        }

        public Task<List<Dto.FileMeta>> GetAllFilesName()
        {
            var container = _blobServiceClient.GetBlobContainerClient(Settings.BlobContainerName);
            var blobs = container.GetBlobs();
            var files = new List<Dto.FileMeta>();
            foreach (var blob in blobs)
            {
                var tableName = Path.GetFileNameWithoutExtension(blob.Name.Replace("_", ""));
                var table = _cloudTableClient.GetTableReference(tableName);

                var rangeQuery = new TableQuery()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Path.GetFileNameWithoutExtension(blob.Name)));

                var description = table.ExecuteQuerySegmentedAsync(rangeQuery, new TableContinuationToken()).Result.Results[0].RowKey;

                files.Add(new Dto.FileMeta(blob.Name, description));
            }
            return Task.FromResult(files);
        }
    }
}
