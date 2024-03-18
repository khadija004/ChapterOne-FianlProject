using ChapterOneApp.Areas.Admin.ViewModels;
using ChapterOneApp.Data;
using ChapterOneApp.Helpers;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IBlogService _blogService;
        public BlogController(AppDbContext context,
                                IWebHostEnvironment env,
                                IBlogService blogService)

        {
            _context = context;
            _env = env;
            _blogService = blogService;
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> bLogs = await _context.Blogs.Include(c => c.Compiler).Where(m => !m.SoftDelete).ToListAsync();
            return View(bLogs);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Blog bLog = await _context.Blogs.Include(c => c.Compiler).FirstOrDefaultAsync(m => m.Id == id);
            if (bLog is null) return NotFound();
            return View(bLog);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.compiler = await GetCompilerAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM blog)
        {
            try
            {
                ViewBag.compiler = await GetCompilerAsync();

                if (!ModelState.IsValid)
                {
                    return View();
                }


                if (!blog.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }


                if (!blog.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }


                string fileName = Guid.NewGuid().ToString() + "_" + blog.Photo.FileName;

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/blog", fileName);

                await FileHelper.SaveFileAsync(path, blog.Photo);

                var compiler = await _context.Compilers.FirstOrDefaultAsync(m => m.Id == blog.CompilerId);
                Blog newBlog = new()
                {
                    Image = fileName,
                    Title = blog.Title,
                    Compiler = compiler,
                    Description = blog.Description,
                    Message = blog.Message
                };

                await _context.Blogs.AddAsync(newBlog);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private async Task<SelectList> GetCompilerAsync()
        {
            IEnumerable<Compiler> compilers = await _blogService.GetCompilersAsync();
            return new SelectList(compilers, "Id", "Name");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                Blog dbBlog = await _blogService.GetById(id);
                if (dbBlog is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/blog", dbBlog.Image);
                FileHelper.DeleteFile(path);

                _context.Blogs.Remove(dbBlog);
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
            ViewBag.compiler = await GetCompilerAsync();
            if (id == null) return BadRequest();
            Blog dbBlog = await _blogService.GetById(id);
            if (dbBlog is null) return NotFound();

            BlogUpdateVM model = new()
            {
                Image = dbBlog.Image,
                Title = dbBlog.Title,
                Description = dbBlog.Description,
                Message = dbBlog.Message,
                CompilerId = dbBlog.CompilerId,
            };

            return View(model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BlogUpdateVM blogUpdate)
        {
            try
            {
                ViewBag.compiler = await GetCompilerAsync();

                if (id == null) return BadRequest();

                Blog dbBlog = await _blogService.GetById(id);

                if (dbBlog is null) return NotFound();

                BlogUpdateVM model = new()
                {
                    Image = dbBlog.Image,
                    Title = dbBlog.Title,
                    Description = dbBlog.Description,
                    Message = dbBlog.Message,
                    CompilerId = dbBlog.CompilerId,
                };


                //if (!ModelState.IsValid)
                //{
                //    return View(model);
                //}

                if (blogUpdate.Photo != null)
                {
                    if (!blogUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!blogUpdate.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/blog", dbBlog.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + blogUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/blog", fileName);

                    await FileHelper.SaveFileAsync(newPath, blogUpdate.Photo);

                    dbBlog.Image = fileName;
                }
                else
                {
                    Blog blog = new()
                    {
                        Image = dbBlog.Image
                    };
                }


                dbBlog.Title = blogUpdate.Title;
                dbBlog.Description = blogUpdate.Description;

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
