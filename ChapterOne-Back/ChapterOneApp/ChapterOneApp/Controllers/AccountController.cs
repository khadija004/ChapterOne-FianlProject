using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
