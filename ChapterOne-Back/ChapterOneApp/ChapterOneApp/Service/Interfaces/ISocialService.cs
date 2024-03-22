using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface ISocialService
    {
        Task<List<Social>> GetSocialsAsync();
        Task<Social> GetByIdAsync(int? id);
    }
}
