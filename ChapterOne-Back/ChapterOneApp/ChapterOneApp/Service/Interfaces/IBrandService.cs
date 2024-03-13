using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IBrandService
    {
        Task<List<Brand>> GetAllAsync();
        Task<Brand> GetByIdAsync(int? id);
    }
}
