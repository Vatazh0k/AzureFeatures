using Azure;
using Azure.Storage.Blobs.Models;
using logic_app_test.AzureModel;
using logic_app_test.Infrastructure;
using logic_app_test.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace logic_app_test.Services
{
    public class AzureTable : IAzureTable
    {
        private readonly CloudTableClient _cloudTableClient;
        public AzureTable(CloudTableClient cloudTableClient) => _cloudTableClient = cloudTableClient;

        public Task AddFileDescription(string fileDescription, string fileName)
        {
            var tableName = Path.GetFileNameWithoutExtension(fileName.Replace("_", ""));
            var table = _cloudTableClient.GetTableReference(tableName);
            table.CreateIfNotExistsAsync().Wait();
            var insertOperation = TableOperation.Insert(new FileMeta(Path.GetFileNameWithoutExtension(fileName), fileDescription));
            table.ExecuteAsync(insertOperation);
            return Task.CompletedTask;
        }

        public Task<List<Dto.FileMeta>> GetFilesDescription(Pageable<BlobItem> blobs)
        {
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
