namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class BlogUpdateVM
    {
        public IFormFile Photo { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public int CompilerId { get; set; }
    }
}
