using ChapterOneApp.Areas.Admin.ViewModels;
using ChapterOneApp.Data;
using ChapterOneApp.Helpers;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapterOneApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly IAuthorService _authorService;
        private readonly ITagService _tagService;
        private readonly IGenreService _genreService;
        public ProductController(AppDbContext context,
                                IWebHostEnvironment env,
                                IProductService productService,
                                IAuthorService authorService,
                                ITagService tagService,
                                IGenreService genreService)
        {
            _context = context;
            _env = env;
            _productService = productService;
            _authorService = authorService;
            _tagService = tagService;
            _genreService = genreService;
        }


        public async Task<IActionResult> Index(int page = 1, int take = 5, int? cateId = null)
        {
            List<Product> products = await _productService.GetPaginateDatas(page, take, cateId);
            List<ProductListVM> mappedDatas = GetMappedDatas(products);
            int pageCount = await GetPageCountAsync(take);
            Paginate<ProductListVM> paginatedDatas = new(mappedDatas, page, pageCount);
            ViewBag.take = take;
            return View(paginatedDatas);
        }


        private async Task<SelectList> GetGenreAsync()
        {
            IEnumerable<Genre> categories = await _genreService.GetAllAsync();
            return new SelectList(categories, "Id", "Name");
        }


        private async Task<SelectList> GetAuthorAsync()
        {
            IEnumerable<Author> authors = await _authorService.GetAllAsync();
            return new SelectList(authors, "Id", "Name");
        }


        private async Task<SelectList> GetTagAsync()
        {
            IEnumerable<Tag> tags = await _tagService.GetAllAsync();
            return new SelectList(tags, "Id", "Name");
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();
            return (int)Math.Ceiling((decimal)productCount / take);
        }


        private List<ProductListVM> GetMappedDatas(List<Product> products)
        {
            List<ProductListVM> mappedDatas = new();

            foreach (var product in products)
            {
                ProductListVM productVM = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.Image,
                    SKU = product.SKU
                };

                mappedDatas.Add(productVM);

            }

            return mappedDatas;
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Product product = await _productService.GettFullDataById((int)id);
            if (product is null) return NotFound();

            ProductDetailVM model = new()
            {
                Name = product.Name,
                SaleCount = product.SaleCount,
                StockCount = product.StockCount,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                ProductGenres = product.ProductGenres,
                ProductAuthors = product.ProductAuthors,
                ProductTags = product.ProductTags,
                Image = product.Image,
                Rate = product.Rate
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            ViewBag.genres = await GetGenreAsync();
            ViewBag.tags = await GetTagAsync();
            ViewBag.authors = await GetAuthorAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            try
            {
                ViewBag.genres = await GetGenreAsync();
                ViewBag.tags = await GetTagAsync();
                ViewBag.authors = await GetAuthorAsync();

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                Product newProduct = new();
                List<ProductGenre> productGenres = new();
                List<ProductTag> productTags = new();
                List<ProductAuthor> productAuthors = new();


                //main image
                if (!model.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }

                if (!model.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }
                string mainImagefileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string mainImagepath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/shop", mainImagefileName);
                await FileHelper.SaveFileAsync(mainImagepath, model.Photo);
                newProduct.Image = mainImagefileName;

                if (model.GenreIds.Count > 0)
                {
                    foreach (var genreId in model.GenreIds)
                    {
                        ProductGenre productGenre = new()
                        {
                            GenreId = genreId
                        };

                        productGenres.Add(productGenre);
                    }
                    newProduct.ProductGenres = productGenres;
                }
                else
                {
                    ModelState.AddModelError("GenreIds", "Don't be empty");
                    return View();
                }



                if (model.TagIds.Count > 0)
                {
                    foreach (var tagId in model.TagIds)
                    {
                        ProductTag productTag = new()
                        {
                            TagId = tagId
                        };

                        productTags.Add(productTag);
                    }
                    newProduct.ProductTags = productTags;
                }
                else
                {
                    ModelState.AddModelError("TagIds", "Don't be empty");
                    return View();
                }



                if (model.AuthorIds.Count > 0)
                {
                    foreach (var authorId in model.AuthorIds)
                    {
                        ProductAuthor productAuthor = new()
                        {
                            AuthorId = authorId
                        };

                        productAuthors.Add(productAuthor);
                    }
                    newProduct.ProductAuthors = productAuthors;
                }
                else
                {
                    ModelState.AddModelError("AuthorIds", "Don't be empty");
                    return View();
                }

                var convertPrice = decimal.Parse(model.Price);
                Random random = new();

                newProduct.Name = model.Name;
                newProduct.Description = model.Description;
                newProduct.Price = convertPrice;
                newProduct.SKU = model.Name.Substring(0, 3) + "_" + random.Next();

                //await _context.ProductImages.AddRangeAsync(productImages);
                await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }


        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == null) return BadRequest();
                Product dbProduct = await _productService.GetById(id);
                if (dbProduct is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/shop", dbProduct.Image);
                FileHelper.DeleteFile(path);

                _context.Products.Remove(dbProduct);
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
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.genres = await GetGenreAsync();
            ViewBag.tags = await GetTagAsync();
            ViewBag.authors = await GetAuthorAsync();


            if (id == null) return BadRequest();
            Product dbProduct = await _productService.GettFullDataById(id);
            if (dbProduct is null) return NotFound();

            ProductUpdateVM model = new()
            {
                Id = dbProduct.Id,
                Image = dbProduct.Image,
                Name = dbProduct.Name,
                Description = dbProduct.Description,
                Price = dbProduct.Price,
                StockCount = dbProduct.StockCount,
                SKU = dbProduct.SKU,
                TagIds = dbProduct.ProductTags.Select(m => m.Tag.Id).ToList(),
                GenreIds = dbProduct.ProductGenres.Select(m => m.Genre.Id).ToList(),
                AuthorIds = dbProduct.ProductAuthors.Select(m => m.Author.Id).ToList(),
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductUpdateVM model)
        {
            ViewBag.genres = await GetGenreAsync();
            ViewBag.tags = await GetTagAsync();
            ViewBag.authors = await GetAuthorAsync();

            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            if (id is null) return BadRequest();
            Product product = await _productService.GettFullDataById((int)id);
            if (product is null) return NotFound();

            List<ProductGenre> productGenres = new();
            List<ProductTag> productTags = new();
            List<ProductAuthor> productAuthors = new();

            //main image

            if (model.Photo is not null)
            {
                if (!model.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }

                if (!model.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }
                string mainImagefileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string mainImagepath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/shop", mainImagefileName);
                await FileHelper.SaveFileAsync(mainImagepath, model.Photo);
                product.Image = mainImagefileName;
            }
            else
            {
                model.Image = product.Image;
            }



            if (model.GenreIds.Count > 0)
            {
                foreach (var genreId in model.GenreIds)
                {
                    ProductGenre productGenre = new()
                    {
                        GenreId = genreId
                    };

                    productGenres.Add(productGenre);
                }
                product.ProductGenres = productGenres;
            }
            else
            {
                ModelState.AddModelError("CategoryIds", "Don't be empty");
                return View();
            }



            if (model.TagIds.Count > 0)
            {
                foreach (var tagId in model.TagIds)
                {
                    ProductTag productTag = new()
                    {
                        TagId = tagId
                    };

                    productTags.Add(productTag);
                }
                product.ProductTags = productTags;
            }
            else
            {
                ModelState.AddModelError("TagIds", "Don't be empty");
                return View();
            }



            if (model.AuthorIds.Count > 0)
            {
                foreach (var authorId in model.AuthorIds)
                {
                    ProductAuthor productAuthor = new()
                    {
                        AuthorId = authorId
                    };

                    productAuthors.Add(productAuthor);
                }
                product.ProductAuthors = productAuthors;
            }
            else
            {
                ModelState.AddModelError("TagIds", "Don't be empty");
                return View();
            }

            product.Id = model.Id;
            product.Name = model.Name;
            product.Price = model.Price;
            product.StockCount = model.StockCount;
            product.SKU = model.SKU;
            product.Description = model.Description;


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));



        }

    }
}
