using ChapterOneApp.Helpers;
using ChapterOneApp.Models;

namespace ChapterOneApp.ViewModels
{
    public class ShopVM
    {
        public List<Genre> Genres { get; set; }
        public List<Models.Product> Product { get; set; }
        public List<Product> Products { get; set; }
        public List<Author> Authors { get; set; }
        public Paginate<ProductVM> PaginateDatas { get; set; }
        public List<Tag> Tags { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public int CountProducts { get; set; }
        public int Rating { get; set; }
    }
}
