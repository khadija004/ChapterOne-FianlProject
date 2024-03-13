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
    public class WrapperController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IWrapperService _wrapperService;
        public WrapperController(AppDbContext context,
                                IWebHostEnvironment env,
                                IWrapperService wrapperService)

        {
            _context = context;
            _env = env;
            _wrapperService = wrapperService;
        }


        public async Task<IActionResult> Index()
        {
            List<Wrapper> wrappers = await _context.Wrappers.ToListAsync();
            return View(wrappers);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Wrapper wrapper = await _wrapperService.GetByIdAsync(id);
            if (wrapper is null) return NotFound();
            return View(wrapper);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WrapperCreateVM wrapper)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!wrapper.Photos.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }



                if (!wrapper.Photos.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }



                string fileName = Guid.NewGuid().ToString() + "_" + wrapper.Photos.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName);

                await FileHelper.SaveFileAsync(path, wrapper.Photos);

                Wrapper newWrapper = new()
                {
                    Image = fileName,
                    Name = wrapper.Name,
                    Description = wrapper.Description
                };


                await _context.Wrappers.AddAsync(newWrapper);


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
                Wrapper dbWrapper = await _wrapperService.GetByIdAsync(id);
                if (dbWrapper is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbWrapper.Image);
                FileHelper.DeleteFile(path);

                _context.Wrappers.Remove(dbWrapper);
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
            Wrapper dbWrapper = await _wrapperService.GetByIdAsync(id);
            if (dbWrapper is null) return NotFound();

            WrapperUpdateVM model = new()
            {
                Image = dbWrapper.Image,
                Description = dbWrapper.Description,
                Name = dbWrapper.Name
            };

            return View(model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, WrapperUpdateVM wrapperUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Wrapper dbWrapper = await _wrapperService.GetByIdAsync(id);

                if (dbWrapper is null) return NotFound();

                WrapperUpdateVM model = new()
                {
                    Image = dbWrapper.Image,
                    Description = dbWrapper.Description,
                    Name = dbWrapper.Name
                };


                //if (!ModelState.IsValid)
                //{
                //    return View(model);
                //}

                if (wrapperUpdate.Photo != null)
                {
                    if (!wrapperUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!wrapperUpdate.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbWrapper.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + wrapperUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName);

                    await FileHelper.SaveFileAsync(newPath, wrapperUpdate.Photo);

                    dbWrapper.Image = fileName;
                }
                else
                {
                    Wrapper wrapper = new()
                    {
                        Image = dbWrapper.Image
                    };
                }


                dbWrapper.Description = wrapperUpdate.Description;
                dbWrapper.Name = wrapperUpdate.Name;

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
