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
    public class AzureStorage
    {
        private AzureBlob _azureBlolbService;
        private AzureTable _azureTableService;


        public AzureBlob Blob => _azureBlolbService;
        public AzureTable Table => _azureTableService;


        public AzureStorage(
            AzureBlob _azureBlolbService,
            AzureTable _azureTableService)
        {
            this._azureBlolbService = _azureBlolbService;
            this._azureTableService = _azureTableService;
        }
    }
}
