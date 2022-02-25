using Microsoft.AspNetCore.Http;

namespace logic_app_test.ViewModels
{
    public class FileWithDescription
    {
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }
}
