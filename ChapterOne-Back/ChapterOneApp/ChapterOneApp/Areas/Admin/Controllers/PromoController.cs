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
    public class PromoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IPromoService _promoService;

        public PromoController(AppDbContext context,
                                IWebHostEnvironment env,
                                IPromoService promoService)
        {
            _context = context;
            _env = env;
            _promoService = promoService;
        }

        public async Task<IActionResult> Index()
        {
            List<Promo> promos = await _context.Promos.ToListAsync();
            return View(promos);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Promo promo = await _promoService.GetByIdAsync(id);
            if (promo is null) return NotFound();
            return View(promo);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromoCreateVM promo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!promo.Photos.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }



                if (!promo.Photos.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }

                decimal convertedPrice = decimal.Parse(promo.Price.Replace(".", ","));


                string fileName = Guid.NewGuid().ToString() + "_" + promo.Photos.FileName;

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName);

                await FileHelper.SaveFileAsync(path, promo.Photos);

                Promo newPromo = new()
                {
                    Image = fileName,
                    Name = promo.Name,
                    Price = convertedPrice
                };


                await _context.Promos.AddAsync(newPromo);


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
                Promo dbPromo = await _promoService.GetByIdAsync(id);
                if (dbPromo is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", dbPromo.Image);
                FileHelper.DeleteFile(path);

                _context.Promos.Remove(dbPromo);
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
            Promo dbPromo = await _promoService.GetByIdAsync(id);
            if (dbPromo is null) return NotFound();

            PromoUpdateVM model = new()
            {
                Image = dbPromo.Image,
                Price = dbPromo.Price,
                Name = dbPromo.Name
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, PromoUpdateVM promoUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Promo dbPromo = await _promoService.GetByIdAsync(id);

                if (dbPromo is null) return NotFound();

                PromoUpdateVM model = new()
                {
                    Image = dbPromo.Image,
                    Price = dbPromo.Price,
                    Name = dbPromo.Name
                };


                //if (!ModelState.IsValid)
                //{
                //    return View(model);
                //}

                if (promoUpdate.Photo != null)
                {
                    if (!promoUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!promoUpdate.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", promoUpdate.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + promoUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/about", fileName);

                    await FileHelper.SaveFileAsync(newPath, promoUpdate.Photo);

                    dbPromo.Image = fileName;
                }
                else
                {
                    Promo promo = new()
                    {
                        Image = dbPromo.Image
                    };
                }


                dbPromo.Price = promoUpdate.Price;
                dbPromo.Name = promoUpdate.Name;

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
