namespace logic_app_test.Infrastructure
{
    public static class Settings
    {
        public static readonly string BlobContainerName = AppsettingsProvider.GetJsonAppsettingsFile()["ConnectionStrings:blobContainerName"];
        public static readonly string StorageConnectionString = AppsettingsProvider.GetJsonAppsettingsFile()["ConnectionStrings:storageConnectionString"];
        public static readonly string JobThumbprint = AppsettingsProvider.GetJsonAppsettingsFile()["ConnectionStrings:jobThumbprint"];
    }
}
