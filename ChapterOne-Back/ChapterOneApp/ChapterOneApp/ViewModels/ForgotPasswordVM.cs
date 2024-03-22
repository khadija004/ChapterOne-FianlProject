using System.ComponentModel.DataAnnotations;

namespace ChapterOneApp.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
