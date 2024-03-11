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
    public class AutobiographyTwoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IAutobiographyTwoService _autobiographyTwoService;
        public AutobiographyTwoController(AppDbContext context,
                                IWebHostEnvironment env,
                                IAutobiographyTwoService autobiographyTwoService)

        {
            _context = context;
            _env = env;
            _autobiographyTwoService = autobiographyTwoService;
        }


        public async Task<IActionResult> Index()
        {
            List<AutobiographyTwo> autobiographyTwos = await _context.AutobiographyTwos.ToListAsync();
            return View(autobiographyTwos);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            AutobiographyTwo autobiographyTwo = await _autobiographyTwoService.GetByIdAsync(id);
            if (autobiographyTwo is null) return NotFound();
            return View(autobiographyTwo);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutobiographyTwoCreateVM autobiographyTwo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!autobiographyTwo.LargePhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("LargePhoto", "File type must be image");
                    return View();
                }




                if (!autobiographyTwo.LargePhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("LargePhoto", "Image size must be max 200kb");
                    return View();
                }



                if (!autobiographyTwo.SmallPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("SmallPhoto", "File type must be image");
                    return View();
                }



                if (!autobiographyTwo.SmallPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("SmallPhoto", "Image size must be max 200kb");
                    return View();
                }



                string fileName1 = Guid.NewGuid().ToString() + "_" + autobiographyTwo.LargePhoto.FileName;

                string path1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName1);

                await FileHelper.SaveFileAsync(path1, autobiographyTwo.LargePhoto);


                string fileName2 = Guid.NewGuid().ToString() + "_" + autobiographyTwo.SmallPhoto.FileName;

                string path2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName2);

                await FileHelper.SaveFileAsync(path2, autobiographyTwo.SmallPhoto);

                AutobiographyTwo newAutobiographyTwo = new()
                {
                    LargeImage = fileName1,
                    SmallImage = fileName2,
                    Title = autobiographyTwo.Title,
                    Name = autobiographyTwo.Name,
                    Description = autobiographyTwo.Description
                };


                await _context.AutobiographyTwos.AddAsync(newAutobiographyTwo);





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
                AutobiographyTwo dbAutobiography = await _autobiographyTwoService.GetByIdAsync(id);
                if (dbAutobiography is null) return NotFound();

                string path1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbAutobiography.LargeImage);
                FileHelper.DeleteFile(path1);

                string path2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbAutobiography.SmallImage);
                FileHelper.DeleteFile(path2);

                _context.AutobiographyTwos.Remove(dbAutobiography);
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
            AutobiographyTwo dbAutobiographyTwo = await _autobiographyTwoService.GetByIdAsync(id);
            if (dbAutobiographyTwo is null) return NotFound();

            AutobiographyTwoUpdateVM model = new()
            {
                LargeImage = dbAutobiographyTwo.LargeImage,
                SmallImage = dbAutobiographyTwo.SmallImage,
                Title = dbAutobiographyTwo.Title,
                Description = dbAutobiographyTwo.Description,
                Name = dbAutobiographyTwo.Name
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AutobiographyTwoUpdateVM autobiographyTwoUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                AutobiographyTwo dbAutobiography = await _autobiographyTwoService.GetByIdAsync(id);

                if (dbAutobiography is null) return NotFound();

                AutobiographyTwoUpdateVM model = new()
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

                if (autobiographyTwoUpdate.LargePhoto != null)
                {
                    if (!autobiographyTwoUpdate.LargePhoto.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("LargePhoto", "Please choose correct image type");
                        return View(model);
                    }

                    if (!autobiographyTwoUpdate.LargePhoto.CheckFileSize(200))
                    {
                        ModelState.AddModelError("LargePhoto", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbAutobiography.LargeImage);

                    FileHelper.DeleteFile(dbPath1);


                    string fileName1 = Guid.NewGuid().ToString() + "_" + autobiographyTwoUpdate.LargePhoto.FileName;

                    string newPath1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName1);

                    await FileHelper.SaveFileAsync(newPath1, autobiographyTwoUpdate.LargePhoto);

                    dbAutobiography.LargeImage = fileName1;
                }
                else
                {
                    AutobiographyTwo autobiographyTwo = new()
                    {
                        LargeImage = dbAutobiography.LargeImage,
                        SmallImage = dbAutobiography.LargeImage,
                    };
                }



                if (autobiographyTwoUpdate.SmallPhoto != null)
                {
                    if (!autobiographyTwoUpdate.SmallPhoto.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("SmallImage", "Please choose correct image type");
                        return View(model);
                    }

                    if (!autobiographyTwoUpdate.SmallPhoto.CheckFileSize(200))
                    {
                        ModelState.AddModelError("SmallImage", "Image size must be max 200kb");
                        return View(model);
                    }

                    string dbPath2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbAutobiography.SmallImage);

                    FileHelper.DeleteFile(dbPath2);

                    string fileName2 = Guid.NewGuid().ToString() + "_" + autobiographyTwoUpdate.SmallPhoto.FileName;

                    string newPath2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName2);

                    await FileHelper.SaveFileAsync(newPath2, autobiographyTwoUpdate.SmallPhoto);

                    dbAutobiography.SmallImage = fileName2;
                }
                else
                {
                    AutobiographyTwo autobiographyTwo = new()
                    {
                        LargeImage = dbAutobiography.LargeImage,
                        SmallImage = dbAutobiography.LargeImage,
                    };
                }


                dbAutobiography.Title = autobiographyTwoUpdate.Title;
                dbAutobiography.Description = autobiographyTwoUpdate.Description;
                dbAutobiography.Name = autobiographyTwoUpdate.Name;

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
