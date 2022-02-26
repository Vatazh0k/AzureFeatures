using Azure.Storage.Blobs;
using logic_app_test.AzureModel;
using logic_app_test.Infrastructure;
using logic_app_test.ViewModels;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections;
using System.IO;

namespace logic_app_test.Services
{
    public static class AzureStorageService
    {
        public static void AddFileToBlob(FileWithDescription file, BlobServiceClient _blobServiceClient)
        {
            var blob = _blobServiceClient.GetBlobContainerClient(Settings.BlobContainerName);
            using var fileStream = file.File.OpenReadStream();
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)file.File.Length);
            fileStream.Position = 0;
            blob.UploadBlob(Path.GetFileNameWithoutExtension(file.File.FileName), fileStream);
        }

        public static byte[] ReadFileFromBlob(string fileName, BlobServiceClient _blobServiceClient)
        {
            var blob = _blobServiceClient.GetBlobContainerClient(Settings.BlobContainerName);
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
            TableOperation insertOperation = TableOperation.Insert(new FileMeta(Path.GetFileNameWithoutExtension(file.File.FileName), file.Description));
            table.ExecuteAsync(insertOperation);
        }

        public static IEnumerable GetAllFilesName(BlobServiceClient _blobServiceClient, CloudTableClient _cloudTableClient)
        {
            var container = _blobServiceClient.GetBlobContainerClient(Settings.BlobContainerName);
            var blobs = container.GetBlobs();
            foreach (var blob in blobs)
            {
                var tableName = Path.GetFileNameWithoutExtension(blob.Name.Replace("_", ""));
                CloudTable table = _cloudTableClient.GetTableReference(tableName);

                TableQuery rangeQuery = new TableQuery()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Path.GetFileNameWithoutExtension(blob.Name)));

                var description = table.ExecuteQuerySegmentedAsync(rangeQuery, new TableContinuationToken()).Result.Results[0].RowKey;

                yield return new Dto.FileMeta(blob.Name, description);
            }
        }
    }
}
