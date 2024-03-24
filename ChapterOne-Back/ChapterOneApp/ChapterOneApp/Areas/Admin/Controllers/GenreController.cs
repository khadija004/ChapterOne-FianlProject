using ChapterOneApp.Areas.Admin.ViewModels;
using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class GenreController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IGenreService _genreService;
        public GenreController(AppDbContext context,
                                IWebHostEnvironment env,
                                IGenreService genreService)
        {
            _context = context;
            _env = env;
            _genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            List<Genre> genres = await _genreService.GetAllAsync();
            return View(genres);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Genre genre = await _genreService.GetByIdAsync(id);
            if (genre is null) return NotFound();
            return View(genre);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreCreateVM genre)
        {
            try
            {

                Genre newGenre = new()
                {
                    Name = genre.Name
                };


                await _context.Genres.AddAsync(newGenre);



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
                Genre dbGenre = await _genreService.GetByIdAsync(id);
                if (dbGenre is null) return NotFound();

                _context.Genres.Remove(dbGenre);
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
            Genre dbGenre = await _genreService.GetByIdAsync(id);
            if (dbGenre is null) return NotFound();

            GenreUpdateVM model = new()
            {
                Name = dbGenre.Name
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, GenreUpdateVM genreUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Genre dbGenre = await _genreService.GetByIdAsync(id);

                if (dbGenre is null) return NotFound();

                GenreUpdateVM model = new()
                {
                    Name = dbGenre.Name
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }



                dbGenre.Name = genreUpdate.Name;

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
