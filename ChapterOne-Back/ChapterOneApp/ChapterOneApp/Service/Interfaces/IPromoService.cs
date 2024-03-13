using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IPromoService
    {
        Task<List<Promo>> GetAllAsync();
        Task<Promo> GetByIdAsync(int? id);
    }
}
