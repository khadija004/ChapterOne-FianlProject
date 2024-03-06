using ChapterOneApp.Areas.Admin.Views.ViewModels;
using ChapterOneApp.Data;
using ChapterOneApp.Helpers;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SliderController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ISliderService _sliderService;

        public SliderController(AppDbContext context,
                        IWebHostEnvironment env,
                        ISliderService sliderService)
        {
            _context = context;
            _env = env;
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();
            return View(sliders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM slider)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!slider.Photos.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }




                if (!slider.Photos.CheckFileSize(800))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 800kb");
                    return View();
                }



                string fileName = Guid.NewGuid().ToString() + "_" + slider.Photos.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName);

                await FileHelper.SaveFileAsync(path, slider.Photos);

                Slider newSlider = new()
                {
                    Image = fileName,
                    Title = slider.Title,
                    Name = slider.Name,
                    Description = slider.Description
                };


                await _context.Sliders.AddAsync(newSlider);





                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
