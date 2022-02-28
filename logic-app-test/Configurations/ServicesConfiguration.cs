using Azure.Storage.Blobs;
using logic_app_test.Infrastructure;
using logic_app_test.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;

namespace logic_app_test.Configurations
{
    public static class ServicesConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<AzureStorageService>();


            services.AddTransient(provider =>
            {
                return new BlobServiceClient(Settings.StorageConnectionString);
            });
            services.AddTransient(provider =>
            {
                return CloudStorageAccount.Parse(Settings.StorageConnectionString).CreateCloudTableClient();
            });
        }
    }
}
