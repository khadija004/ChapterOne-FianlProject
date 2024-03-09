using System.ComponentModel.DataAnnotations;

namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class TeamCreateVM
    {
        [Required(ErrorMessage = "Don`t be empty")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public int PositionId { get; set; } = new();
    }
}
