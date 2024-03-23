using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCommentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        public ProductCommentController(AppDbContext context,
                                        IProductService product)
        {
            _context = context;
            _productService = product;
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _productService.GetComments();
            return View(comments);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            ProductComment dbcomment = await _productService.GetCommentByIdWithProduct((int)id);
            if (dbcomment is null) return NotFound();
            return View(dbcomment);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                ProductComment dbcomment = await _productService.GetCommentById((int)id);
                if (dbcomment is null) return NotFound();

                _context.ProductComments.Remove(dbcomment);
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
