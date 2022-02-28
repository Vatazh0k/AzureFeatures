using logic_app_test.Interfaces;

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
