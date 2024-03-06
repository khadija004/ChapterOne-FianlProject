using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
