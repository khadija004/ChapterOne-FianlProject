using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IGenreService
    {
        Task<List<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int? id);
    }
}
