using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IStoreService
    {
        Task<List<Store>> GetAllAsync();
        Task<Store> GetByIdAsync(int? id);
    }
}
