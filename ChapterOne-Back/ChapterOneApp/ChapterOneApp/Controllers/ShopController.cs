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
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IGenreService _genreService;
        private readonly IProductService _productService;
        private readonly ITagService _tagService;
        private readonly IAuthorService _authorService;
        private readonly ICartService _cartService;
        private readonly IWishlistService _wishlistService;
        private readonly ILayoutService _layoutService;

        public ShopController(AppDbContext context,
                              IGenreService genreService,
                              IProductService productService,
                              IAuthorService authorService,
                              ITagService tagService,
                              ICartService cartService,
                              IWishlistService wishlistService,
                              ILayoutService layoutService)

        {
            _context = context;
            _genreService = genreService;
            _productService = productService;
            _authorService = authorService;
            _tagService = tagService;
            _cartService = cartService;
            _wishlistService = wishlistService;
            _layoutService = layoutService;
        }


        public async Task<IActionResult> Index(int page = 1, int take = 6, string sortValue = null, string searchText = null, int? genreId = null, int? authorId = null, int? tagId = null, int? value1 = null, int? value2 = null)
        {
            List<Product> datas = await _productService.GetPaginatedDatasAsync(page, take, sortValue, searchText, genreId, authorId, tagId, value1, value2);
            List<ProductVM> mappedDatas = GetMappedDatas(datas);
            ViewBag.genreId = genreId;
            ViewBag.tagId = tagId;
            ViewBag.authorId = authorId;
            ViewBag.value1 = value1;
            ViewBag.value2 = value2;
            ViewBag.searchText = searchText;
            ViewBag.sortValue = sortValue;

            int pageCount = 0;


            if (sortValue != null)
            {
                pageCount = await GetPageCountAsync(take, sortValue, null, null, null, null, null, null);
            }
            if (genreId != null)
            {
                pageCount = await GetPageCountAsync(take, null, null, genreId, null, null, null, null);
            }
            if (authorId != null)
            {
                pageCount = await GetPageCountAsync(take, null, null, null, authorId, null, null, null);
            }
            if (tagId != null)
            {
                pageCount = await GetPageCountAsync(take, null, null, null, null, tagId, null, null);
            }
            if (value1 != null && value2 != null)
            {
                pageCount = await GetPageCountAsync(take, null, null, null, null, null, value1, value2);
            }

            if (sortValue == null && searchText == null && genreId == null && authorId == null && tagId == null && value1 == null && value2 == null)
            {
                pageCount = await GetPageCountAsync(take, null, null, null, null, null, null, null);
            }

            Paginate<ProductVM> paginatedDatas = new(mappedDatas, page, pageCount);

            List<Tag> tags = await _tagService.GetAllAsync();
            List<Models.Author> authors = await _authorService.GetAllAsync();
            List<Genre> genres = await _genreService.GetAllAsync();

            ShopVM model = new()
            {
                Products = await _productService.GetAll(),
                Tags = tags,
                Genres = genres,
                Authors = authors,
                HeaderBackgrounds = _layoutService.GetHeaderBackgroundData(),
                PaginateDatas = paginatedDatas,
                CountProducts = await _productService.GetCountAsync(),
            };
            return View(model);
        }


        private async Task<int> GetPageCountAsync(int take, string sortValue, string searchText, int? genreId, int? authorId, int? tagId, int? value1, int? value2)
        {
            int prodCount = 0;
            if (sortValue is not null)
            {
                prodCount = await _productService.GetProductsCountBySortTextAsync(sortValue);
            }
            //if (searchText is not null)
            //{
            //    prodCount = await _productService.GetProductsCountBySearchTextAsync(searchText);
            //}
            if (genreId is not null)
            {
                prodCount = await _productService.GetProductsCountByGenreAsync(genreId);
            }
            if (authorId is not null)
            {
                prodCount = await _productService.GetProductsCountByAuthorAsync(authorId);
                prodCount = await _productService.GetProductsCountByAuthorAsync(authorId);
            }
            if (tagId is not null)
            {
                prodCount = await _productService.GetProductsCountByTagAsync(tagId);
            }
            if (value1 != null && value2 != null)
            {
                prodCount = await _productService.GetProductsCountByRangeAsync(value1, value2); ;
            }
            if (sortValue == null && searchText == null && genreId == null && tagId == null && authorId == null && value1 == null && value2 == null)
            {
                prodCount = await _productService.GetCountAsync();
            }

            return (int)Math.Ceiling((decimal)prodCount / take);
        }


        private List<ProductVM> GetMappedDatas(List<Product> products)
        {
            List<ProductVM> mappedDatas = new();
            foreach (var product in products)
            {
                ProductVM productList = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.Image,
                };
                mappedDatas.Add(productList);
            }
            return mappedDatas;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts(int page = 1, int take = 6)
        {
            int pageCount = await GetPageCountAsync(take, null, null, null, null, null, null, null);
            var products = await _productService.GetPaginatedDatasAsync(page, take, null, null, null, null, null, null, null);
            List<ProductVM> mappedDatas = GetMappedDatas(products);
            Paginate<ProductVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return PartialView("_ProductsPartial", paginatedDatas);
        }


        [HttpGet]
        public async Task<IActionResult> GetRangeProducts(int value1, int value2, int page = 1, int take = 6)
        {
            List<Product> products = await _context.Products.Where(x => x.Price >= value1 && x.Price <= value2).ToListAsync();
            ViewBag.value1 = value1;
            ViewBag.value2 = value2;
            var productCount = products.Count();
            int pageCount = (int)Math.Ceiling((decimal)productCount / take);
            List<ProductVM> mappedDatas = GetMappedDatas(products);
            Paginate<ProductVM> paginatedDatas = new(mappedDatas, page, pageCount);
            return PartialView("_ProductsPartial", paginatedDatas);
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsByGenre(int? id, int page = 1, int take = 6)
        {
            if (id is null) return BadRequest();
            ViewBag.genreId = id;

            var products = await _productService.GetProductsByGenreIdAsync(id, page, take);

            int pageCount = await GetPageCountAsync(take, null, null, (int)id, null, null, null, null);

            Paginate<ProductVM> model = new(products, page, pageCount);

            return PartialView("_ProductsPartial", model);
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsByTag(int? id, int page = 1, int take = 6)
        {
            if (id is null) return BadRequest();
            ViewBag.tagId = id;

            var products = await _productService.GetProductsByTagIdAsync(id);

            int pageCount = await GetPageCountAsync(take, null, null, null, null, (int)id, null, null);

            Paginate<ProductVM> model = new(products, page, pageCount);

            return PartialView("_ProductsPartial", model);
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsByAuthor(int? id, int page = 1, int take = 6)
        {
            if (id is null) return BadRequest();
            ViewBag.authorId = id;
            var products = await _productService.GetProductsByAuthorIdAsync(id);
            int pageCount = await GetPageCountAsync(take, null, null, null, (int)id, null, null, null);

            Paginate<ProductVM> model = new(products, page, pageCount);

            return PartialView("_ProductsPartial", model);
        }


        public async Task<IActionResult> Sort(string? sortValue, int page = 1, int take = 6)
        {
            List<Product> products = new();

            if (sortValue == "1")
            {
                products = await _context.Products.ToListAsync();
            };
            if (sortValue == "2")
            {
                products = await _context.Products.OrderByDescending(p => p.SaleCount).ToListAsync();

            };
            if (sortValue == "3")
            {
                products = await _context.Products.OrderByDescending(p => p.Rate).ToListAsync();

            };
            if (sortValue == "4")
            {
                products = await _context.Products.OrderByDescending(p => p.CreateDate).ToListAsync();

            };
            if (sortValue == "5")
            {
                products = await _context.Products.OrderBy(p => p.Price).ToListAsync();

            };
            if (sortValue == "6")
            {
                products = await _context.Products.OrderByDescending(p => p.Price).ToListAsync();

            };
            int pageCount = await GetPageCountAsync(take, sortValue, null, null, null, null, null, null);
            List<ProductVM> mappedDatas = GetMappedDatas(products);
            Paginate<ProductVM> model = new(mappedDatas, page, pageCount);

            return PartialView("_ProductsPartial", model);
        }


        //public async Task<IActionResult> MainSearch(string searchText)
        //{
        //    var products = await _context.Products
        //                        .Include(m => m.ProductGenres)?
        //                        .OrderByDescending(m => m.Id)
        //                        .Where(m => !m.SoftDelete && m.Name.ToLower().Trim().Contains(searchText.ToLower().Trim()))
        //                        .ToListAsync();

        //    return View(products);
        //}


        //public async Task<IActionResult> Search(string searchText)
        //{
        //    List<Product> products = await _context.Products
        //                                    .Include(m => m.ProductGenres)
        //                                    .Include(m => m.ProductAuthors)
        //                                    .Include(m => m.ProductTags)
        //                                    .Where(m => m.Name.ToLower().Contains(searchText.ToLower()))
        //                                    .Take(5)
        //                                    .ToListAsync();
        //    return PartialView("_SearchPartial", products);
        //}


        public async Task<IActionResult> ProductDetail(int? id)
        {
            Product productDt = await _productService.GettFullDataById((int)id);
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Genre> genres = await _genreService.GetAllAsync();
            List<Product> releatedProducts = new();

            List<ProductComment> productComments = await _context.ProductComments.Include(m => m.AppUser).Where(m => m.ProductId == id).ToListAsync();
            CommentVM commentVM = new CommentVM();

            foreach (var genre in genres)
            {
                Product releatedProduct = await _context.ProductGenres.Include(m => m.Product)
                                                .Where(m => m.Genre.Id == genre.Id)
                                                .Select(m => m.Product)
                                                .FirstOrDefaultAsync();
                if (releatedProduct != null)
                {
                    releatedProducts.Add(releatedProduct);
                }
            }


            ProductDetailVM model = new()
            {
                ProductDt = productDt,
                HeaderBackgrounds = headerBackgrounds,
                RelatedProducts = releatedProducts,
                CommentVM = commentVM,
                ProductComments = productComments
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> PostComment(ProductDetailVM productDetailVM, string userId, int productId)
        {
            if (productDetailVM.CommentVM.Message == null)
            {
                ModelState.AddModelError("Message", "Don't empty");
                return RedirectToAction(nameof(ProductDetail), new { id = productId });
            }

            ProductComment productComment = new()
            {
                FullName = productDetailVM.CommentVM?.FullName,
                Email = productDetailVM.CommentVM?.Email,
                Subject = productDetailVM.CommentVM?.Subject,
                Message = productDetailVM.CommentVM?.Message,
                AppUserId = userId,
                ProductId = productId
            };

            await _context.ProductComments.AddAsync(productComment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProductDetail), new { id = productId });

        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id is null) return BadRequest();

            Product dbProduct = await _productService.GetById((int)id);

            if (dbProduct == null) return NotFound();

            List<CartVM> carts = _cartService.GetDatasFromCookie();

            CartVM existProduct = carts.FirstOrDefault(p => p.ProductId == id);

            _cartService.SetDatasToCookie(carts, dbProduct, existProduct);

            int cartCount = carts.Count;

            return Ok(cartCount);
        }


        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int? id)
        {
            if (id is null) return BadRequest();

            Product dbProduct = await _productService.GetById((int)id);

            if (dbProduct == null) return NotFound();

            List<WishlistVM> wishlists = _wishlistService.GetDatasFromCookie();

            WishlistVM existProduct = wishlists.FirstOrDefault(p => p.ProductId == id);

            _wishlistService.SetDatasToCookie(wishlists, dbProduct, existProduct);

            int wishlistCount = wishlists.Count;

            return Ok(wishlistCount);
        }
    }
}
