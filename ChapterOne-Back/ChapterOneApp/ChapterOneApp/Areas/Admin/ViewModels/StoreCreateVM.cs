using System.ComponentModel.DataAnnotations;

namespace ChapterOneApp.Areas.Admin.ViewModels
{
    public class StoreCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Mail { get; set; }
    }
}
