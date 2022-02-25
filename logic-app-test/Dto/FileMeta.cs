namespace logic_app_test.Dto
{
    public class FileMeta
    {
        public string FileName { get; set; }
        public string Description { get; set; }

        public FileMeta(string fileName, string description)
        {
            FileName = fileName;
            Description = description;
        }
    }
}
