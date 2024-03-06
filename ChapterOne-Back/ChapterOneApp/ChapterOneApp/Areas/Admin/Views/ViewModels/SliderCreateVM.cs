using System.ComponentModel.DataAnnotations;

namespace ChapterOneApp.Areas.Admin.Views.ViewModels
{
    public class SliderCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public IFormFile Photos { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Description { get; set; }
    }
}
