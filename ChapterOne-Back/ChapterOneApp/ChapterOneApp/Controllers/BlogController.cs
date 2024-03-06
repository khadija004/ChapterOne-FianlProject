using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
