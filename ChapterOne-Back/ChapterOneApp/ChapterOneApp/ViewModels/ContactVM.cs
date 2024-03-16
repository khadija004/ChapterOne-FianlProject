using ChapterOneApp.Models;

namespace ChapterOneApp.ViewModels
{
    public class ContactVM
    {
        public List<Store> Stores { get; set; }
        public List<Brand> Brands { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
    }
}
