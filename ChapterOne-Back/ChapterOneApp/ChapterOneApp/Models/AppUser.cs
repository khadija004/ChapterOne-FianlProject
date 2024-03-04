using Microsoft.AspNetCore.Identity;

namespace ChapterOneApp.Models
{
    public class AppUser: IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsRememberMe { get; set; }
        public ICollection<ProductComment> ProductComments { get; set; }
        public ICollection<BlogComment> BlogComments { get; set; }
    }
}
