using Azure.Storage.Blobs;
using logic_app_test.AzureModel;
using logic_app_test.Infrastructure;
using logic_app_test.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.IO;

namespace logic_app_test.Services
{
    public static class AzureStorageService
    {
        public static void AddFileToBlob(IFormFile file, BlobServiceClient _blobServiceClient)
        {
            var blob = _blobServiceClient.GetBlobContainerClient(AppsettingsProvider.GetJsonAppsettingsFile()["ConnectionStrings:blobContainerName"]);
            using var fileStream = file.OpenReadStream();
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)file.Length);
            fileStream.Position = 0;
            blob.UploadBlob(Path.GetFileNameWithoutExtension(file.FileName), fileStream);
        }

        public static byte[] ReadFileFromBlob(string fileName, BlobServiceClient _blobServiceClient)
        {
            var blob = _blobServiceClient.GetBlobContainerClient(AppsettingsProvider.GetJsonAppsettingsFile()["ConnectionStrings:blobContainerName"]);
            using (var ms = new MemoryStream())
            {
                var file = blob.GetBlobClient(fileName);
                file.DownloadTo(ms);
                return ms.ToArray();
            }
        }

        public static void AddDescriptionToTable(FileWithDescription file, CloudTableClient cloudTableClient)
        {
            var tableName = Path.GetFileNameWithoutExtension(file.File.FileName.Replace("_", ""));
            var table = cloudTableClient.GetTableReference(tableName);
            table.CreateIfNotExistsAsync().Wait();
            TableBatchOperation operations = new TableBatchOperation();
            TableOperation insertOperation = TableOperation.Insert(new FileMeta(file.File.FileName, file.Description));
            table.ExecuteAsync(insertOperation);
        }

        public static IEnumerable GetAllFilesName(BlobServiceClient _blobServiceClient)
        {
            var container = _blobServiceClient.GetBlobContainerClient(AppsettingsProvider.GetJsonAppsettingsFile()["ConnectionStrings:blobContainerName"]);
            var blobs = container.GetBlobs();
            foreach (var blob in blobs) yield return blob.Name;    
        }
    }
}
