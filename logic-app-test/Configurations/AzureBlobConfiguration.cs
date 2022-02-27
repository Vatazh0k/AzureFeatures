using Azure.Storage.Blobs;
using logic_app_test.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace logic_app_test.Configurations
{
    public static class AzureBlobConfiguration
    {
        public static void AddBlob(this IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                return new BlobServiceClient(Settings.StorageConnectionString);
            });
        }
    }
}
