using ChapterOneApp.Models;

namespace ChapterOneApp.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Rating { get; set; }
        public ICollection<ProductAuthor> ProductAuthors { get; set; }
    }
}
