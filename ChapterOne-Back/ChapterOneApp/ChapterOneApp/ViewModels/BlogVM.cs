using ChapterOneApp.Helpers;
using ChapterOneApp.Models;

namespace ChapterOneApp.ViewModels
{
    public class BlogVM
    {
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public List<Blog> BLogs { get; set; }
        public Paginate<Blog> PaginateBlog { get; set; }
        public List<Product> NewProduct { get; set; }
    }
}
