using ChapterOneApp.Areas.Admin.ViewModels;
using ChapterOneApp.Data;
using ChapterOneApp.Helpers;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
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


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                Slider dbSlider = await _sliderService.GetById(id);
                if (dbSlider is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbSlider.Image);
                FileHelper.DeleteFile(path);

                _context.Sliders.Remove(dbSlider);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            Slider dbSlider = await _sliderService.GetById(id);
            if (dbSlider is null) return NotFound();

            SliderUpdateVM model = new()
            {
                Image = dbSlider.Image,
                Title = dbSlider.Title,
                Description = dbSlider.Description,
                Name = dbSlider.Name
            };

            return View(model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderUpdateVM sliderUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Slider dbSlider = await _sliderService.GetById(id);

                if (dbSlider is null) return NotFound();

                SliderUpdateVM model = new()
                {
                    Image = dbSlider.Image,
                    Title = dbSlider.Title,
                    Description = dbSlider.Description,
                    Name = dbSlider.Name
                };


                //if (!ModelState.IsValid)
                //{
                //    return View(model);
                //}

                if (sliderUpdate.Photo != null)
                {
                    if (!sliderUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!sliderUpdate.Photo.CheckFileSize(800))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 800kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbSlider.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + sliderUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName);

                    await FileHelper.SaveFileAsync(newPath, sliderUpdate.Photo);

                    dbSlider.Image = fileName;
                }
                else
                {
                    Slider slider = new()
                    {
                        Image = dbSlider.Image
                    };
                }


                dbSlider.Title = sliderUpdate.Title;
                dbSlider.Description = sliderUpdate.Description;
                dbSlider.Name = sliderUpdate.Name;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
                return View();
            }
        }



        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Slider slider = await _sliderService.GetById(id);
            if (slider is null) return NotFound();
            return View(slider);
        }
    }
}
