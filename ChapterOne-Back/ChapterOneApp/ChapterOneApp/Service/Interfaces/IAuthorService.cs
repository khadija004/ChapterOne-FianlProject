using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int? id);
    }
}
