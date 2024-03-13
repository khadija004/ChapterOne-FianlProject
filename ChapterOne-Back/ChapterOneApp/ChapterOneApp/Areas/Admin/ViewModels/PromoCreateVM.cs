using System.ComponentModel.DataAnnotations;

namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class PromoCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public IFormFile Photos { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public string Price { get; set; }
    }
}
