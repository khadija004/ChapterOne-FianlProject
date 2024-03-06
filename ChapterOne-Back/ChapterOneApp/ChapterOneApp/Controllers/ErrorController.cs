using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
