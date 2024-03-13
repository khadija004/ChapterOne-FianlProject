using ChapterOneApp.Models;

namespace ChapterOneApp.ViewModels
{
    public class AboutVM
    {
        public List<Wrapper> Wrappers { get; set; }
        public List<AutobiographyThree> AutobiographyThrees { get; set; }
        public List<AutobiographyFour> AutobiographyFours { get; set; }
        public List<Promo> Promos { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }


    }
}
