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
    public class AutobiographyFourController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IAutobiographyFourService _autobiographyFourService;
        public AutobiographyFourController(AppDbContext context,
                                IWebHostEnvironment env,
                                IAutobiographyFourService autobiographyFourService)

        {
            _context = context;
            _env = env;
            _autobiographyFourService = autobiographyFourService;
        }


        public async Task<IActionResult> Index()
        {
            List<AutobiographyFour> autobiographyFours = await _context.AutobiographyFours.ToListAsync();
            return View(autobiographyFours);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            AutobiographyFour autobiographyFour = await _autobiographyFourService.GetByIdAsync(id);
            if (autobiographyFour is null) return NotFound();
            return View(autobiographyFour);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutobiographyFourCreateVM autobiographyFour)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!autobiographyFour.LargePhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("LargePhoto", "File type must be image");
                    return View();
                }




                if (!autobiographyFour.LargePhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("LargePhoto", "Image size must be max 200kb");
                    return View();
                }



                if (!autobiographyFour.SmallPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("SmallPhoto", "File type must be image");
                    return View();
                }



                if (!autobiographyFour.SmallPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("SmallPhoto", "Image size must be max 200kb");
                    return View();
                }



                string fileName1 = Guid.NewGuid().ToString() + "_" + autobiographyFour.LargePhoto.FileName;

                string path1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName1);

                await FileHelper.SaveFileAsync(path1, autobiographyFour.LargePhoto);


                string fileName2 = Guid.NewGuid().ToString() + "_" + autobiographyFour.SmallPhoto.FileName;

                string path2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName2);

                await FileHelper.SaveFileAsync(path2, autobiographyFour.SmallPhoto);

                AutobiographyFour newAutobiographyFour = new()
                {
                    LargeImage = fileName1,
                    SmallImage = fileName2,
                    Title = autobiographyFour.Title,
                    Name = autobiographyFour.Name,
                    Description = autobiographyFour.Description
                };


                await _context.AutobiographyFours.AddAsync(newAutobiographyFour);





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
                AutobiographyFour dbAutobiography = await _autobiographyFourService.GetByIdAsync(id);
                if (dbAutobiography is null) return NotFound();

                string path1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbAutobiography.LargeImage);
                FileHelper.DeleteFile(path1);

                string path2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbAutobiography.SmallImage);
                FileHelper.DeleteFile(path2);

                _context.AutobiographyFours.Remove(dbAutobiography);
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
            AutobiographyFour dbAutobiographyFour = await _autobiographyFourService.GetByIdAsync(id);
            if (dbAutobiographyFour is null) return NotFound();

            AutobiographyFourUpdateVM model = new()
            {
                LargeImage = dbAutobiographyFour.LargeImage,
                SmallImage = dbAutobiographyFour.SmallImage,
                Title = dbAutobiographyFour.Title,
                Description = dbAutobiographyFour.Description,
                Name = dbAutobiographyFour.Name
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AutobiographyFourUpdateVM autobiographyFourUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                AutobiographyFour dbAutobiography = await _autobiographyFourService.GetByIdAsync(id);

                if (dbAutobiography is null) return NotFound();

                AutobiographyFourUpdateVM model = new()
                {
                    SmallImage = dbAutobiography.SmallImage,
                    LargeImage = dbAutobiography.LargeImage,
                    Title = dbAutobiography.Title,
                    Description = dbAutobiography.Description,
                    Name = dbAutobiography.Name
                };


                //if (!ModelState.IsValid)
                //{
                //    return View(model);
                //}

                if (autobiographyFourUpdate.LargePhoto != null)
                {
                    if (!autobiographyFourUpdate.LargePhoto.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("LargePhoto", "Please choose correct image type");
                        return View(model);
                    }

                    if (!autobiographyFourUpdate.LargePhoto.CheckFileSize(200))
                    {
                        ModelState.AddModelError("LargePhoto", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbAutobiography.LargeImage);

                    FileHelper.DeleteFile(dbPath1);


                    string fileName1 = Guid.NewGuid().ToString() + "_" + autobiographyFourUpdate.LargePhoto.FileName;

                    string newPath1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName1);

                    await FileHelper.SaveFileAsync(newPath1, autobiographyFourUpdate.LargePhoto);

                    dbAutobiography.LargeImage = fileName1;
                }
                else
                {
                    AutobiographyFour autobiographyFour = new()
                    {
                        LargeImage = dbAutobiography.LargeImage,
                        SmallImage = dbAutobiography.LargeImage,
                    };
                }



                if (autobiographyFourUpdate.SmallPhoto != null)
                {
                    if (!autobiographyFourUpdate.SmallPhoto.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("SmallImage", "Please choose correct image type");
                        return View(model);
                    }

                    if (!autobiographyFourUpdate.SmallPhoto.CheckFileSize(200))
                    {
                        ModelState.AddModelError("SmallImage", "Image size must be max 200kb");
                        return View(model);
                    }

                    string dbPath2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbAutobiography.SmallImage);

                    FileHelper.DeleteFile(dbPath2);

                    string fileName2 = Guid.NewGuid().ToString() + "_" + autobiographyFourUpdate.SmallPhoto.FileName;

                    string newPath2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName2);

                    await FileHelper.SaveFileAsync(newPath2, autobiographyFourUpdate.SmallPhoto);

                    dbAutobiography.SmallImage = fileName2;
                }
                else
                {
                    AutobiographyFour autobiographyFour = new()
                    {
                        LargeImage = dbAutobiography.LargeImage,
                        SmallImage = dbAutobiography.LargeImage,
                    };
                }


                dbAutobiography.Title = autobiographyFourUpdate.Title;
                dbAutobiography.Description = autobiographyFourUpdate.Description;
                dbAutobiography.Name = autobiographyFourUpdate.Name;

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
