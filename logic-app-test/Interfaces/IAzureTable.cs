using Azure;
using Azure.Storage.Blobs.Models;
using logic_app_test.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace logic_app_test.Interfaces
{
    public interface IAzureTable
    {
        public Task AddFileDescription(string fileDescription, string fileName);
        public Task<List<FileMeta>> GetFilesDescription(List<string> blobs);
    }
}
