using ChapterOneApp.Models;
using ChapterOneApp.ViewModels;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetById(int id);
        Task<List<Product>> GetAll();
        Task<int> GetCountAsync();
        Task<List<Product>> GetFeaturedProducts();
        Task<List<Product>> GetBestsellerProducts();
        Task<List<Product>> GetLatestProducts();
        Task<List<Product>> GetNewProducts();
        Task<Product> GetFullDataById(int id);
        Task<Product> GettFullDataById(int id);
        Task<List<Product>> GetPaginateDatas(int page, int take, int? cateId);
        Task<List<ProductComment>> GetComments();
        Task<ProductComment> GetCommentByIdWithProduct(int? id);
        Task<ProductComment> GetCommentById(int? id);
        Task<List<Product>> GetPaginatedDatasAsync(int page, int take, string sortValue, string searchText, int? genreId, int? authorId, int? tagId, int? value1, int? value2);
        Task<int> GetProductsCountByRangeAsync(int? value1, int? value2);
        Task<int> GetProductsCountBySearchTextAsync(string searchText);
        Task<int> GetProductsCountBySortTextAsync(string sortValue);
        Task<int> GetProductsCountByGenreAsync(int? genreId);
        Task<int> GetProductsCountByAuthorAsync(int? authorId);
        Task<int> GetProductsCountByTagAsync(int? tagId);
        Task<List<ProductVM>> GetProductsByGenreIdAsync(int? id, int page = 1, int take = 9);
        Task<List<ProductVM>> GetProductsByAuthorIdAsync(int? id, int page = 1, int take = 9);
        Task<List<ProductVM>> GetProductsByTagIdAsync(int? id, int page = 1, int take = 9);
    }
}
