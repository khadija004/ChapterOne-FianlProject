using ChapterOneApp.Areas.Admin.ViewModels;
using ChapterOneApp.Data;
using ChapterOneApp.Helpers;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class OurController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IOurService _ourService;
        public OurController(AppDbContext context,
                                IWebHostEnvironment env,
                                IOurService ourService)
        {
            _context = context;
            _env = env;
            _ourService = ourService;
        }

        public async Task<IActionResult> Index()
        {
            List<Our> ours = await _context.Ours.ToListAsync();
            return View(ours);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Our our = await _ourService.GetByIdAsync(id);
            if (our is null) return NotFound();
            return View(our);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OurCreateVM our)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!our.Photos.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }



                if (!our.Photos.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }



                string fileName = Guid.NewGuid().ToString() + "_" + our.Photos.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName);

                await FileHelper.SaveFileAsync(path, our.Photos);

                Our newOur = new()
                {
                    Image = fileName,
                    Name = our.Name,
                    Description = our.Description
                };


                await _context.Ours.AddAsync(newOur);


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
                Our dbOur = await _ourService.GetByIdAsync(id);
                if (dbOur is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbOur.Image);
                FileHelper.DeleteFile(path);

                _context.Ours.Remove(dbOur);
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
            Our dbOur = await _ourService.GetByIdAsync(id);
            if (dbOur is null) return NotFound();

            OurUpdateVM model = new()
            {
                Image = dbOur.Image,
                Description = dbOur.Description,
                Name = dbOur.Name
            };

            return View(model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, OurUpdateVM ourUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Our dbOur = await _ourService.GetByIdAsync(id);

                if (dbOur is null) return NotFound();

                OurUpdateVM model = new()
                {
                    Image = dbOur.Image,
                    Description = dbOur.Description,
                    Name = dbOur.Name
                };


                //if (!ModelState.IsValid)
                //{
                //    return View(model);
                //}

                if (ourUpdate.Photo != null)
                {
                    if (!ourUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!ourUpdate.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbOur.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + ourUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName);

                    await FileHelper.SaveFileAsync(newPath, ourUpdate.Photo);

                    dbOur.Image = fileName;
                }
                else
                {
                    Our our = new()
                    {
                        Image = dbOur.Image
                    };
                }


                dbOur.Description = ourUpdate.Description;
                dbOur.Name = ourUpdate.Name;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
                return View();
            }
        }

    }
}
