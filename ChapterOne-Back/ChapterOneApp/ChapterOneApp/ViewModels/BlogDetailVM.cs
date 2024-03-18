using ChapterOneApp.Models;

namespace ChapterOneApp.ViewModels
{
    public class BlogDetailVM
    {
        public Blog BlogDt { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Product> NewProducts { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public CommentVM CommentVM { get; set; }
        public List<BlogComment> BlogComments { get; set; }
    }
}
