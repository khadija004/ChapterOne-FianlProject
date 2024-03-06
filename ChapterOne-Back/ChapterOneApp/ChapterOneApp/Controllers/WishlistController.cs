using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
