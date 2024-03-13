using System.ComponentModel.DataAnnotations;

namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class BrandCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public IFormFile Photo { get; set; }
    }
}
