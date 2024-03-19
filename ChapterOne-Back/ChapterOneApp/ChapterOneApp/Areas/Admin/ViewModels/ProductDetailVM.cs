using ChapterOneApp.Models;

namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class ProductDetailVM
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Rate { get; set; } = 5;
        public int SaleCount { get; set; }
        public int StockCount { get; set; }
        public string SKU { get; set; }
        public ICollection<ProductGenre> ProductGenres { get; set; }
        public ICollection<ProductAuthor> ProductAuthors { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
    }
}
