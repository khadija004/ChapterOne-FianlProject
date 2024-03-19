using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(int? id);
    }
}
