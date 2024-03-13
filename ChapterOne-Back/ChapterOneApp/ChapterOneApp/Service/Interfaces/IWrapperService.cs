using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IWrapperService
    {
        Task<List<Wrapper>> GetAllAsync();
        Task<Wrapper> GetByIdAsync(int? id);
    }
}
