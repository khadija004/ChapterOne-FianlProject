using System.ComponentModel.DataAnnotations;

namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class TeamUpdateVM
    {
        public string Image { get; set; }
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public int PositionId { get; set; }
        public string Description { get; set; }
    }
}
