using ChapterOneApp.Areas.Admin.ViewModels;
using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IStoreService _storeService;
        public StoreController(AppDbContext context,
                                IWebHostEnvironment env,
                                IStoreService storeService)
        {
            _context = context;
            _env = env;
            _storeService = storeService;
        }

        public async Task<IActionResult> Index()
        {
            List<Store> stores = await _context.Stores.ToListAsync();
            return View(stores);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Store store = await _storeService.GetByIdAsync(id);
            if (store is null) return NotFound();
            return View(store);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoreCreateVM store)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }


                Store newStore = new()
                {
                    Location = store.Location,
                    Phone = store.Phone,
                    Mail = store.Mail
                };


                await _context.Stores.AddAsync(newStore);


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
                Store dbStore = await _storeService.GetByIdAsync(id);
                if (dbStore is null) return NotFound();

                _context.Stores.Remove(dbStore);
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
            Store dbStore = await _storeService.GetByIdAsync(id);
            if (dbStore is null) return NotFound();

            StoreUpdateVM model = new()
            {
                Location = dbStore.Location,
                Phone = dbStore.Phone,
                Mail = dbStore.Mail
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, StoreUpdateVM storeUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Store dbStore = await _storeService.GetByIdAsync(id);

                if (dbStore is null) return NotFound();

                StoreUpdateVM model = new()
                {
                    Location = dbStore.Location,
                    Phone = dbStore.Phone,
                    Mail = dbStore.Mail
                };


                //if (!ModelState.IsValid)
                //{
                //    return View(model);
                //}

                dbStore.Location = storeUpdate.Location;
                dbStore.Phone = storeUpdate.Phone;
                dbStore.Mail = storeUpdate.Mail;

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
