using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class PromoService : IPromoService
    {
        private readonly AppDbContext _context;
        public PromoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Promo>> GetAllAsync()
        {
            return await _context.Promos.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Promo> GetByIdAsync(int? id)
        {
            return await _context.Promos.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
