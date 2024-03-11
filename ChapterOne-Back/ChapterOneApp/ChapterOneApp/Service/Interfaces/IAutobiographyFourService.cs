using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IAutobiographyFourService
    {
        Task<List<AutobiographyFour>> GetAllAsync();
        Task<AutobiographyFour> GetByIdAsync(int? id);
    }
}
