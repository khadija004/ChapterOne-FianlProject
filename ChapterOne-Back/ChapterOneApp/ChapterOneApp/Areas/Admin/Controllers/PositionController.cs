using ChapterOneApp.Areas.Admin.ViewModels;
using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class PositionController : Controller
    {

        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        readonly IPositionService _positionService;

        public PositionController(AppDbContext context,
                        IWebHostEnvironment env,
                        IPositionService positionService)
        {
            _context = context;
            _env = env;
            _positionService = positionService;
        }

        public async Task<IActionResult> Index()
        {
            List<Position> positions = await _positionService.GetAllAsync();
            return View(positions);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositionCreateVM position)
        {
            try
            {

                Position newPosition = new()
                {
                    Name = position.Name
                };


                await _context.Positions.AddAsync(newPosition);



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
                Position dbPosition = await _positionService.GetByIdAsync(id);
                if (dbPosition is null) return NotFound();

                _context.Positions.Remove(dbPosition);
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
            Position dbPosition = await _positionService.GetByIdAsync(id);
            if (dbPosition is null) return NotFound();

            PositionUpdateVM model = new()
            {
                Name = dbPosition.Name
            };

            return View(model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, PositionUpdateVM positionUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Position dbPosition = await _positionService.GetByIdAsync(id);

                if (dbPosition is null) return NotFound();

                PositionUpdateVM model = new()
                {
                    Name = dbPosition.Name
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }



                dbPosition.Name = positionUpdate.Name;

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
