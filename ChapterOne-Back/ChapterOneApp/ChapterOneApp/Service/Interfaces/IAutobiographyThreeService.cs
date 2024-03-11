using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IAutobiographyThreeService
    {
        Task<List<AutobiographyThree>> GetAllAsync();
        Task<AutobiographyThree> GetByIdAsync(int? id);
    }
}
