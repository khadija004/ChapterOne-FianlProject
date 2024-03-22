namespace ChapterOneApp.ViewModels
{
    public class LayoutVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public int BasketCount { get; set; }
        public int WishlistCount { get; set; }
        public IEnumerable<CartVM> CartVMs { get; set; }
        public IEnumerable<ShopVM> ShopVMs { get; set; }
    }
}
