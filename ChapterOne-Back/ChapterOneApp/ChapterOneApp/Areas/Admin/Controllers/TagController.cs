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
    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ITagService _tagService;
        public TagController(AppDbContext context,
                                IWebHostEnvironment env,
                                ITagService tagService)
        {
            _context = context;
            _env = env;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            List<Tag> tags = await _tagService.GetAllAsync();
            return View(tags);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Tag tag = await _tagService.GetByIdAsync(id);
            if (tag is null) return NotFound();
            return View(tag);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagCreateVM tag)
        {
            try
            {

                Tag newTag = new()
                {
                    Name = tag.Name
                };


                await _context.Tags.AddAsync(newTag);



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
                Tag dbTag = await _tagService.GetByIdAsync(id);
                if (dbTag is null) return NotFound();

                _context.Tags.Remove(dbTag);
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
            Tag dbTag = await _tagService.GetByIdAsync(id);
            if (dbTag is null) return NotFound();

            TagUpdateVM model = new()
            {
                Name = dbTag.Name
            };
            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, TagUpdateVM tagUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Tag dbTag = await _tagService.GetByIdAsync(id);

                if (dbTag is null) return NotFound();

                TagUpdateVM model = new()
                {
                    Name = dbTag.Name
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }



                dbTag.Name = tagUpdate.Name;

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
