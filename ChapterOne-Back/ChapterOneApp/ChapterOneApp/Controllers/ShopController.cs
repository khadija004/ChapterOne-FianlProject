using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
