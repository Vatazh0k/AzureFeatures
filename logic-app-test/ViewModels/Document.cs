using Microsoft.AspNetCore.Http;

namespace logic_app_test.ViewModels
{
    public class Document
    {
        public string Name { get => Content.FileName; }
        public string Description { get; set; }
        public IFormFile Content { get; set; }
    }
}
