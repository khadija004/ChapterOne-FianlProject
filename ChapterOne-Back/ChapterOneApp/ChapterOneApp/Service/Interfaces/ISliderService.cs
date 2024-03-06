using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface ISliderService
    {
        Task<List<Slider>> GetAll();
        Task<Slider> GetById(int? id);
    }
}
