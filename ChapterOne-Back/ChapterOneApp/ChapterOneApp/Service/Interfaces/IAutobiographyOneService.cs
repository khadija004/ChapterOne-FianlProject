using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IAutobiographyOneService
    {
        Task<List<AutobiographyOne>> GetAllAsync();
        Task<AutobiographyOne> GetByIdAsync(int? id);
    }
}
