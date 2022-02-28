using Azure.Storage.Blobs;
using logic_app_test.AzureModel;
using logic_app_test.Infrastructure;
using logic_app_test.Interfaces;
using logic_app_test.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace logic_app_test.Services
{
    public class AzureStorage
    {
        private IAzureBlob _azureBlolbService;
        private IAzureTable _azureTableService;


        public IAzureBlob Blob => _azureBlolbService;
        public IAzureTable Table => _azureTableService;


        public AzureStorage(
            IAzureBlob azureBlolbService,
            IAzureTable azureTableService)
        {
            _azureBlolbService = azureBlolbService;
            _azureTableService = azureTableService;
        }
    }
}
