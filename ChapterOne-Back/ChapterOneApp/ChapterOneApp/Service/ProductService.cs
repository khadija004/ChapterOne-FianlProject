using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using ChapterOneApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAll() => await _context.Products
                                                                        .Include(m => m.ProductTags)
                                                                        .ThenInclude(m => m.Tag)
                                                                        .Include(m => m.ProductComments)
                                                                        .Include(m => m.ProductGenres)
                                                                        .ThenInclude(m => m.Genre)
                                                                        .Include(m => m.ProductAuthors)
                                                                        .ThenInclude(m => m.Author)
                                                                        .ToListAsync();
        public async Task<Product> GettFullDataById(int id)
        {
            var a = await _context.Products
                                        .Include(m => m.ProductTags)
                                        .ThenInclude(m => m.Tag)
                                        .Include(m => m.ProductAuthors)
                                        .ThenInclude(m => m.Author)
                                        .Include(m => m.ProductComments)
                                        .Include(m => m.ProductGenres)
                                        .ThenInclude(m => m.Genre)
                                        .FirstOrDefaultAsync(m => m.Id == id);
            return a;
        }
        public async Task<Product> GetById(int id) => await _context.Products.FindAsync(id);
        public async Task<int> GetCountAsync() => await _context.Products.CountAsync();
        public async Task<List<Product>> GetFeaturedProducts() => await _context.Products.Where(m => !m.SoftDelete).OrderByDescending(m => m.Rate).ToListAsync();
        public async Task<List<Product>> GetBestsellerProducts() => await _context.Products.Where(m => !m.SoftDelete).OrderByDescending(m => m.SaleCount).ToListAsync();
        public async Task<List<Product>> GetLatestProducts() => await _context.Products.Where(m => !m.SoftDelete).OrderByDescending(m => m.CreateDate).ToListAsync();
        public async Task<List<Product>> GetNewProducts() => await _context.Products.Where(m => !m.SoftDelete).OrderByDescending(m => m.CreateDate).Take(4).ToListAsync();
        public async Task<Product> GetFullDataById(int id) => await _context.Products.Include(m => m.ProductGenres).FirstOrDefaultAsync(m => m.Id == id);
        public async Task<List<Product>> GetPaginateDatas(int page, int take, int? cateId)
        {
            List<Product> products = null;

            if (cateId == null)
            {
                products = await _context.Products.

            Include(m => m.ProductGenres)?.
            Include(m => m.ProductTags).
            Include(m => m.ProductAuthors).
            Include(m => m.ProductComments).Where(m => !m.SoftDelete).
            Skip((page * take) - take).
            Take(take).ToListAsync();
            }
            else
            {
                products = await _context.ProductGenres.Include(m => m.Product).Where(m => m.Genre.Id == cateId).Select(m => m.Product).Where(m => !m.SoftDelete).
                Skip((page * take) - take).
                Take(take).ToListAsync();
            }

            return products;


        }
        public async Task<List<ProductComment>> GetComments()
        {
            return await _context.ProductComments.Include(p => p.Product).ToListAsync();
        }
        public async Task<ProductComment> GetCommentByIdWithProduct(int? id)
        {
            return await _context.ProductComments.Include(p => p.Product).FirstOrDefaultAsync(pc => pc.Id == id);
        }
        public async Task<ProductComment> GetCommentById(int? id)
        {
            return await _context.ProductComments.FirstOrDefaultAsync(pc => pc.Id == id);
        }
        public async Task<List<Product>> GetPaginatedDatasAsync(int page, int take, string sortValue, string searchText, int? genreId, int? tagId, int? authorId, int? value1, int? value2)
        {
            List<Product> products = products = await _context.Products
                                                            .Include(p => p.ProductGenres)
                                                            .ThenInclude(pc => pc.Genre)
                                                            .Include(p => p.ProductAuthors)
                                                            .ThenInclude(ps => ps.Author)
                                                            .Include(p => p.ProductTags)
                                                            .ThenInclude(ps => ps.Tag)
                                                            .Skip((page * take) - take)
                                                            .Take(take)
                                                            .ToListAsync();


            if (searchText != null)
            {
                products = await _context.Products
                .OrderByDescending(p => p.Id)
                .Where(p => p.Name.ToLower().Contains(searchText.ToLower()))
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();
            }
            if (genreId != null)
            {
                return await _context.ProductGenres
                .Include(p => p.Product)
                .Where(pc => pc.Genre.Id == genreId)
                .Select(p => p.Product)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();
            }
            if (authorId != null)
            {
                return await _context.ProductAuthors
                        .Include(p => p.Product)
                        .Where(pc => pc.Author.Id == authorId)
                        .Select(p => p.Product)
                        .Skip((page * take) - take)
                        .Take(take)
                        .ToListAsync();
            }
            if (tagId != null)
            {
                return await _context.ProductTags
                .Include(p => p.Product)
                .Where(pc => pc.Tag.Id == tagId)
                .Skip((page * take) - take)
                .Take(take)
                .Select(p => p.Product)
                .ToListAsync();
            }

            if (value1 != null && value2 != null)
            {
                products = await _context.Products
               .Include(p => p.Image)
               .Where(p => p.Price >= value1 && p.Price <= value2)
               .Skip((page * take) - take)
               .Take(take)
               .ToListAsync();

            }


            return products;
        }
        public async Task<int> GetProductsCountByRangeAsync(int? value1, int? value2)
        {
            return await _context.Products.Where(p => p.Price >= value1 && p.Price <= value2)
                                 .CountAsync();
        }
        public async Task<int> GetProductsCountBySearchTextAsync(string searchText)
        {
            return await _context.Products.Where(p => p.Name.ToLower().Contains(searchText.ToLower()))
                                 .Include(p => p.Image)
                                 .CountAsync();
        }
        public async Task<int> GetProductsCountBySortTextAsync(string sortValue)
        {
            int count = 0;
            if (sortValue == "1")
            {
                return await _context.Products.Include(m => m.Image).CountAsync();
            };
            if (sortValue == "2")
            {
                count = await _context.Products.OrderByDescending(p => p.SaleCount).CountAsync();
            };
            if (sortValue == "3")
            {
                count = await _context.Products.OrderByDescending(p => p.Rate).CountAsync();
            };
            if (sortValue == "4")
            {
                count = await _context.Products.OrderByDescending(p => p.CreateDate).CountAsync();
            };
            if (sortValue == "5")
            {
                count = await _context.Products.OrderByDescending(p => p.Price).CountAsync();
            };
            if (sortValue == "6")
            {
                count = await _context.Products.OrderBy(p => p.Price).CountAsync();
            };

            return count;
        }
        public async Task<int> GetProductsCountByGenreAsync(int? genreId)
        {
            return await _context.ProductGenres
                 .Include(p => p.Product)
                 .Where(pc => pc.Genre.Id == genreId)
                 .Select(p => p.Product)
                 .CountAsync();
        }
        public async Task<int> GetProductsCountByAuthorAsync(int? authorId)
        {
            return await _context.ProductAuthors
                 .Include(p => p.Product)
                 .Where(pc => pc.Author.Id == authorId)
                 .Select(p => p.Product)
                 .CountAsync();
        }
        public async Task<int> GetProductsCountByTagAsync(int? tagId)
        {
            return await _context.ProductTags
                  .Include(p => p.Product)
                  .Where(pc => pc.Tag.Id == (int)tagId)
                  .Select(p => p.Product)
                  .CountAsync();
        }
        public async Task<List<ProductVM>> GetProductsByGenreIdAsync(int? id, int page = 1, int take = 9)
        {
            List<ProductVM> model = new();
            var products = await _context.ProductGenres
                .Include(p => p.Product)
                .Where(pc => pc.Genre.Id == id)
                .Select(p => p.Product)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            foreach (var item in products)
            {
                model.Add(new ProductVM
                {
                    Id = item.Id,
                    Price = item.Price,
                    Name = item.Name,
                    Image = item.Image,
                    Rating = item.Rate
                });
            }
            return model;
        }
        public async Task<List<ProductVM>> GetProductsByAuthorIdAsync(int? id, int page = 1, int take = 9)
        {
            List<ProductVM> model = new();
            var products = await _context.ProductAuthors
                .Include(p => p.Product)
                .Where(pc => pc.Author.Id == id)
                .Select(p => p.Product)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();
            foreach (var item in products)
            {
                model.Add(new ProductVM
                {
                    Id = item.Id,
                    Price = item.Price,
                    Name = item.Name,
                    Image = item.Image,
                    Rating = item.Rate
                });
            }
            return model;
        }
        public async Task<List<ProductVM>> GetProductsByTagIdAsync(int? id, int page = 1, int take = 9)
        {
            List<ProductVM> model = new();
            var products = await _context.ProductTags
                .Include(p => p.Product)
                .Where(pc => pc.Tag.Id == id)
                .Select(p => p.Product)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            foreach (var item in products)
            {
                model.Add(new ProductVM
                {
                    Id = item.Id,
                    Price = item.Price,
                    Name = item.Name,
                    Image = item.Image,
                    Rating = item.Rate
                });
            }
            return model;
        }

    }
}
