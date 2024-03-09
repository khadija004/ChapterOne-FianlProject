using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IOurService
    {

        Task<List<Our>> GetAllAsync();
        Task<Our> GetByIdAsync(int? id);
    }
}
