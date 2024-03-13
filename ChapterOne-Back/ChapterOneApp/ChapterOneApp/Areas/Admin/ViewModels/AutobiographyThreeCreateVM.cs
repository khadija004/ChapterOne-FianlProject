using System.ComponentModel.DataAnnotations;

namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class AutobiographyThreeCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public IFormFile LargePhoto { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public IFormFile SmallPhoto { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Description { get; set; }
    }
}
