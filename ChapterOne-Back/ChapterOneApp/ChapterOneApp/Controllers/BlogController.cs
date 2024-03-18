using ChapterOneApp.Data;
using ChapterOneApp.Helpers;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using ChapterOneApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlogService _blogService;
        private readonly IProductService _productService;

        public BlogController(AppDbContext context,
                              IBlogService blogService,
                              IProductService productService)
        {
            _context = context;
            _blogService = blogService;
            _productService = productService;
        }


        public async Task<IActionResult> Index(int page = 1, int take = 2)
        {
            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            var blogs = await _blogService.GetBlogs();
            List<Blog> paginateProduct = await _blogService.GetPaginateDatas(page, take);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Blog> paginateDatas = new(paginateProduct, page, pageCount);

            List<Product> newProducts = await _productService.GetNewProducts();

            BlogVM model = new()
            {
                HeaderBackgrounds = headerBackground,
                BLogs = blogs.ToList(),
                PaginateBlog = paginateDatas,
                NewProduct = newProducts.ToList()
            };

            return View(model);
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var blogCount = await _blogService.GetCountAsync();
            return (int)Math.Ceiling((decimal)blogCount / take);
        }


        public async Task<IActionResult> BlogDetail(int? id)
        {
            if (id is null) return BadRequest();    


            Blog blogDt = await _blogService.GetById((int)id);
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Blog> blogs = await _blogService.GetBlogs();
            if (blogDt is null) return NotFound();

            List<Product> newProduct = await _productService.GetNewProducts();


            List<BlogComment> blogComments = await _context.BlogComments.Include(m => m.AppUser).Where(m => m.BlogId == id).ToListAsync();
            CommentVM commentVM = new CommentVM();


            BlogDetailVM model = new()
            {
                BlogDt = blogDt,
                HeaderBackgrounds = headerBackgrounds,
                BlogComments = blogComments,
                CommentVM = commentVM,
                Blogs = blogs,
                NewProducts = newProduct
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> PostComment(BlogDetailVM blogDetailVM, string userId, int blogId)
        {
            if (blogDetailVM.CommentVM.Message == null)
            {
                ModelState.AddModelError("Message", "Don't empty");
                return RedirectToAction(nameof(BlogDetail), new { id = blogId });
            }

            BlogComment blogComment = new()
            {
                FullName = blogDetailVM.CommentVM?.FullName,
                Email = blogDetailVM.CommentVM?.Email,
                Subject = blogDetailVM.CommentVM?.Subject,
                Message = blogDetailVM.CommentVM?.Message,
                AppUserId = userId,
                BlogId = blogId
            };

            await _context.BlogComments.AddAsync(blogComment);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(BlogDetail), new { id = blogId });

        }
    }
}
