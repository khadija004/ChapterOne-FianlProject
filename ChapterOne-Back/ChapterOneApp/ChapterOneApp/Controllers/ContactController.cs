using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
