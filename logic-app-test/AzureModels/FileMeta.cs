using Microsoft.WindowsAzure.Storage.Table;

namespace logic_app_test.AzureModel
{
    public class FileMeta : TableEntity
    {
        public string FileName { get; set; }
        public string Description { get; set; }

        public FileMeta(string fileName, string description) : base(fileName, description)
        {
            FileName = fileName;
            Description = description;
        }
    }
}
