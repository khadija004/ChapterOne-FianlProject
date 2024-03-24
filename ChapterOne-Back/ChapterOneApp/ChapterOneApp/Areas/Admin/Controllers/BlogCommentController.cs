using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BlogCommentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlogService _blogService;
        public BlogCommentController(AppDbContext context,
                                     IBlogService blogService)
        {
            _blogService = blogService;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var comments = await _blogService.GetComments();
            return View(comments);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            BlogComment dbcomment = await _blogService.GetCommentByIdWithBlog((int)id);
            if (dbcomment is null) return NotFound();
            return View(dbcomment);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                BlogComment dbcomment = await _blogService.GetCommentById((int)id);
                if (dbcomment is null) return NotFound();

                _context.BlogComments.Remove(dbcomment);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }
    }
}
