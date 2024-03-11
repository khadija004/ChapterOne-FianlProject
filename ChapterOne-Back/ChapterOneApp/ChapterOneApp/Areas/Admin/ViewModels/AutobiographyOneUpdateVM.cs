namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class AutobiographyOneUpdateVM
    {
        public IFormFile LargePhoto { get; set; }
        public IFormFile SmallPhoto { get; set; }
        public string LargeImage { get; set; }
        public string SmallImage { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
