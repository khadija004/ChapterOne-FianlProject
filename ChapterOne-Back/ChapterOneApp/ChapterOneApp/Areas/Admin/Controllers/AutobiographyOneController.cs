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
    public class AutobiographyOneController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IAutobiographyOneService _autobiographyOneService;
        public AutobiographyOneController(AppDbContext context,
                                IWebHostEnvironment env,
                                IAutobiographyOneService autobiographyOneService)

        {
            _context = context;
            _env = env;
            _autobiographyOneService = autobiographyOneService;
        }


        public async Task<IActionResult> Index()
        {
            List<AutobiographyOne> autobiographyOnes = await _context.AutobiographyOnes.ToListAsync();
            return View(autobiographyOnes);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            AutobiographyOne autobiographyOne = await _autobiographyOneService.GetByIdAsync(id);
            if (autobiographyOne is null) return NotFound();
            return View(autobiographyOne);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutobiographyOneCreateVM autobiographyOne)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!autobiographyOne.LargePhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("LargePhoto", "File type must be image");
                    return View();
                }




                if (!autobiographyOne.LargePhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("LargePhoto", "Image size must be max 200kb");
                    return View();
                }



                if (!autobiographyOne.SmallPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("SmallPhoto", "File type must be image");
                    return View();
                }



                if (!autobiographyOne.SmallPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("SmallPhoto", "Image size must be max 200kb");
                    return View();
                }



                string fileName1 = Guid.NewGuid().ToString() + "_" + autobiographyOne.LargePhoto.FileName;

                string path1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName1);

                await FileHelper.SaveFileAsync(path1, autobiographyOne.LargePhoto);


                string fileName2 = Guid.NewGuid().ToString() + "_" + autobiographyOne.SmallPhoto.FileName;

                string path2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName2);

                await FileHelper.SaveFileAsync(path2, autobiographyOne.SmallPhoto);

                AutobiographyOne newAutobiographyOne = new()
                {
                    LargeImage = fileName1,
                    SmallImage = fileName2,
                    Title = autobiographyOne.Title,
                    Name = autobiographyOne.Name,
                    Description = autobiographyOne.Description
                };


                await _context.AutobiographyOnes.AddAsync(newAutobiographyOne);





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
                AutobiographyOne dbAutobiography = await _autobiographyOneService.GetByIdAsync(id);
                if (dbAutobiography is null) return NotFound();

                string path1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbAutobiography.LargeImage);
                FileHelper.DeleteFile(path1);

                string path2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbAutobiography.SmallImage);
                FileHelper.DeleteFile(path2);

                _context.AutobiographyOnes.Remove(dbAutobiography);
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
            AutobiographyOne dbAutobiographyOne = await _autobiographyOneService.GetByIdAsync(id);
            if (dbAutobiographyOne is null) return NotFound();

            AutobiographyOneUpdateVM model = new()
            {
                LargeImage = dbAutobiographyOne.LargeImage,
                SmallImage = dbAutobiographyOne.SmallImage,
                Title = dbAutobiographyOne.Title,
                Description = dbAutobiographyOne.Description,
                Name = dbAutobiographyOne.Name
            };

            return View(model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AutobiographyOneUpdateVM autobiographyOneUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                AutobiographyOne dbAutobiography = await _autobiographyOneService.GetByIdAsync(id);

                if (dbAutobiography is null) return NotFound();

                AutobiographyOneUpdateVM model = new()
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

                if (autobiographyOneUpdate.LargePhoto != null)
                {
                    if (!autobiographyOneUpdate.LargePhoto.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("LargePhoto", "Please choose correct image type");
                        return View(model);
                    }

                    if (!autobiographyOneUpdate.LargePhoto.CheckFileSize(200))
                    {
                        ModelState.AddModelError("LargePhoto", "Image size must be max 200kb");
                        return View(model);
                    }




                    string dbPath1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbAutobiography.LargeImage);

                    FileHelper.DeleteFile(dbPath1);


                    string fileName1 = Guid.NewGuid().ToString() + "_" + autobiographyOneUpdate.LargePhoto.FileName;

                    string newPath1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName1);

                    await FileHelper.SaveFileAsync(newPath1, autobiographyOneUpdate.LargePhoto);

                    dbAutobiography.LargeImage = fileName1;

                }
                else
                {
                    AutobiographyOne autobiographyOne = new()
                    {
                        LargeImage = dbAutobiography.LargeImage,
                        SmallImage = dbAutobiography.LargeImage,
                    };
                }



                if (autobiographyOneUpdate.SmallPhoto != null)
                {
                    if (!autobiographyOneUpdate.SmallPhoto.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("SmallImage", "Please choose correct image type");
                        return View(model);
                    }

                    if (!autobiographyOneUpdate.SmallPhoto.CheckFileSize(200))
                    {
                        ModelState.AddModelError("SmallImage", "Image size must be max 200kb");
                        return View(model);
                    }

                    string dbPath2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", dbAutobiography.SmallImage);

                    FileHelper.DeleteFile(dbPath2);


                    string fileName2 = Guid.NewGuid().ToString() + "_" + autobiographyOneUpdate.SmallPhoto.FileName;

                    string newPath2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/home", fileName2);

                    await FileHelper.SaveFileAsync(newPath2, autobiographyOneUpdate.SmallPhoto);

                    dbAutobiography.SmallImage = fileName2;
                }
                else
                {
                    AutobiographyOne autobiographyOne = new()
                    {
                        LargeImage = dbAutobiography.LargeImage,
                        SmallImage = dbAutobiography.LargeImage,
                    };
                }


                dbAutobiography.Title = autobiographyOneUpdate.Title;
                dbAutobiography.Description = autobiographyOneUpdate.Description;
                dbAutobiography.Name = autobiographyOneUpdate.Name;

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
