using ChapterOneApp.Models;

namespace ChapterOneApp.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Our> Ours { get; set; }
        public List<Team> Teams { get; set; }
        public List<AutobiographyOne> AutobiographyOnes { get; set; }
        public List<AutobiographyTwo> AutobiographyTwos { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Product> Products { get; set; }
        public List<Genre> Genres { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }




    }
}
