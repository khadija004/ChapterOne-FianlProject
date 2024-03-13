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
    public class AutobiographyThreeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IAutobiographyThreeService _autobiographyThreeService;
        public AutobiographyThreeController(AppDbContext context,
                                IWebHostEnvironment env,
                                IAutobiographyThreeService autobiographyThreeService)

        {
            _context = context;
            _env = env;
            _autobiographyThreeService = autobiographyThreeService;
        }


        public async Task<IActionResult> Index()
        {
            List<AutobiographyThree> autobiographyThrees = await _context.AutobiographyThrees.ToListAsync();
            return View(autobiographyThrees);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            AutobiographyThree autobiographyThree = await _autobiographyThreeService.GetByIdAsync(id);
            if (autobiographyThree is null) return NotFound();
            return View(autobiographyThree);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutobiographyThreeCreateVM autobiographyThree)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!autobiographyThree.LargePhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("LargePhoto", "File type must be image");
                    return View();
                }




                if (!autobiographyThree.LargePhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("LargePhoto", "Image size must be max 200kb");
                    return View();
                }



                if (!autobiographyThree.SmallPhoto.CheckFileType("image/"))
                {
                    ModelState.AddModelError("SmallPhoto", "File type must be image");
                    return View();
                }



                if (!autobiographyThree.SmallPhoto.CheckFileSize(200))
                {
                    ModelState.AddModelError("SmallPhoto", "Image size must be max 200kb");
                    return View();
                }



                string fileName1 = Guid.NewGuid().ToString() + "_" + autobiographyThree.LargePhoto.FileName;

                string path1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName1);

                await FileHelper.SaveFileAsync(path1, autobiographyThree.LargePhoto);


                string fileName2 = Guid.NewGuid().ToString() + "_" + autobiographyThree.SmallPhoto.FileName;

                string path2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName2);

                await FileHelper.SaveFileAsync(path2, autobiographyThree.SmallPhoto);

                AutobiographyThree newAutobiographyThree = new()
                {
                    LargeImage = fileName1,
                    SmallImage = fileName2,
                    Title = autobiographyThree.Title,
                    Name = autobiographyThree.Name,
                    Description = autobiographyThree.Description
                };


                await _context.AutobiographyThrees.AddAsync(newAutobiographyThree);





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
                AutobiographyThree dbAutobiography = await _autobiographyThreeService.GetByIdAsync(id);
                if (dbAutobiography is null) return NotFound();

                string path1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbAutobiography.LargeImage);
                FileHelper.DeleteFile(path1);

                string path2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbAutobiography.SmallImage);
                FileHelper.DeleteFile(path2);

                _context.AutobiographyThrees.Remove(dbAutobiography);
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
            AutobiographyThree dbAutobiographyThree = await _autobiographyThreeService.GetByIdAsync(id);
            if (dbAutobiographyThree is null) return NotFound();

            AutobiographyThreeUpdateVM model = new()
            {
                LargeImage = dbAutobiographyThree.LargeImage,
                SmallImage = dbAutobiographyThree.SmallImage,
                Title = dbAutobiographyThree.Title,
                Description = dbAutobiographyThree.Description,
                Name = dbAutobiographyThree.Name
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AutobiographyThreeUpdateVM autobiographyThreeUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                AutobiographyThree dbAutobiography = await _autobiographyThreeService.GetByIdAsync(id);

                if (dbAutobiography is null) return NotFound();

                AutobiographyThreeUpdateVM model = new()
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

                if (autobiographyThreeUpdate.LargePhoto != null)
                {
                    if (!autobiographyThreeUpdate.LargePhoto.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("LargePhoto", "Please choose correct image type");
                        return View(model);
                    }

                    if (!autobiographyThreeUpdate.LargePhoto.CheckFileSize(200))
                    {
                        ModelState.AddModelError("LargePhoto", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbAutobiography.LargeImage);

                    FileHelper.DeleteFile(dbPath1);


                    string fileName1 = Guid.NewGuid().ToString() + "_" + autobiographyThreeUpdate.LargePhoto.FileName;

                    string newPath1 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName1);

                    await FileHelper.SaveFileAsync(newPath1, autobiographyThreeUpdate.LargePhoto);

                    dbAutobiography.LargeImage = fileName1;
                }
                else
                {
                    AutobiographyThree autobiographyThree = new()
                    {
                        LargeImage = dbAutobiography.LargeImage,
                        SmallImage = dbAutobiography.LargeImage,
                    };
                }



                if (autobiographyThreeUpdate.SmallPhoto != null)
                {
                    if (!autobiographyThreeUpdate.SmallPhoto.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("SmallImage", "Please choose correct image type");
                        return View(model);
                    }

                    if (!autobiographyThreeUpdate.SmallPhoto.CheckFileSize(200))
                    {
                        ModelState.AddModelError("SmallImage", "Image size must be max 200kb");
                        return View(model);
                    }

                    string dbPath2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbAutobiography.SmallImage);

                    FileHelper.DeleteFile(dbPath2);

                    string fileName2 = Guid.NewGuid().ToString() + "_" + autobiographyThreeUpdate.SmallPhoto.FileName;

                    string newPath2 = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName2);

                    await FileHelper.SaveFileAsync(newPath2, autobiographyThreeUpdate.SmallPhoto);

                    dbAutobiography.SmallImage = fileName2;
                }
                else
                {
                    AutobiographyThree autobiographyThree = new()
                    {
                        LargeImage = dbAutobiography.LargeImage,
                        SmallImage = dbAutobiography.LargeImage,
                    };
                }


                dbAutobiography.Title = autobiographyThreeUpdate.Title;
                dbAutobiography.Description = autobiographyThreeUpdate.Description;
                dbAutobiography.Name = autobiographyThreeUpdate.Name;

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
