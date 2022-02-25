using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace logic_app_test.Infrastructure
{
    public class AppsettingsProvider
    {
        public static IConfigurationRoot GetJsonAppsettingsFile() => new ConfigurationBuilder()
             .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
             .AddJsonFile("appsettings.json", false)
             .Build();
    }
}
