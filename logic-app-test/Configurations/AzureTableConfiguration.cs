using logic_app_test.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;

namespace logic_app_test.Configurations
{
    public static class AzureTableConfiguration
    {
        public static void AddTable(this IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                return CloudStorageAccount.Parse(Settings.StorageConnectionString).CreateCloudTableClient();
            });
        }
    }
}
