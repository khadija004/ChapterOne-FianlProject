using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IAutobiographyTwoService
    {
        Task<List<AutobiographyTwo>> GetAllAsync();
        Task<AutobiographyTwo> GetByIdAsync(int? id);
    }
}
