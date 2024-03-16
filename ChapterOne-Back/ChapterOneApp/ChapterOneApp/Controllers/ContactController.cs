using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using ChapterOneApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStoreService _storeService;
        private readonly IBrandService _brandService;
        public ContactController(AppDbContext context,IBrandService brandService,IStoreService storeService)
        {
            _context = context;
            _storeService = storeService;
            _brandService = brandService;
        }


        public async Task<IActionResult> Index()
        {
            List<Store> stores = await _storeService.GetAllAsync();
            List<Brand> brands = await _brandService.GetAllAsync();
            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);

            ContactVM model = new()
            {
                Stores = stores,
                Brands = brands,
                HeaderBackgrounds = headerBackground,
            };

            return View(model);
        }
    }
}
