using Azure;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace logic_app_test.Interfaces
{
    public interface IAzureBlob
    {
        public Task AddFile(IFormFile file);//bytes[] insted of IFromFile
        public Task<byte[]> GetFile(string fileName);
        public Task<List<string>> GetAllFilesName();
    }
}
