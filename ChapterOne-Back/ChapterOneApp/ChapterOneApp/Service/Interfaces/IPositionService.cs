using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IPositionService
    {
        Task<List<Position>> GetAllAsync();
        Task<Position> GetByIdAsync(int? id);
    }
}
