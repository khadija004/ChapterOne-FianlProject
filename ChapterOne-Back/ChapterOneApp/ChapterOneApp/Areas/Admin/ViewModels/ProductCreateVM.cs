using System.ComponentModel.DataAnnotations;

namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class ProductCreateVM
    {
        [Required(ErrorMessage = "Don`t be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public string Price { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> AuthorIds { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> TagIds { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> GenreIds { get; set; }
    }
}
