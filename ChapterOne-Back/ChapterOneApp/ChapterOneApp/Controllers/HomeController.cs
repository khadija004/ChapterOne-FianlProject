using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using ChapterOneApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChapterOneApp.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;
        readonly ISliderService _sliderService;

        public HomeController(AppDbContext context,ISliderService sliderService)
        {
            _context = context;
            _sliderService = sliderService;
        }


        public async Task<IActionResult> Index()
        {

            List<Slider> sliders = await _sliderService.GetAll();

            HomeVM model = new()
            {
                Sliders = sliders,
           
            };

            return View(model);
        }

     
    }
}