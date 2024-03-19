namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public string SKU { get; set; }
        public List<int> AuthorIds { get; set; }
        public List<int> TagIds { get; set; }
        public List<int> GenreIds { get; set; }
    }
}
