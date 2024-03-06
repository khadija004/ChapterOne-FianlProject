using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
