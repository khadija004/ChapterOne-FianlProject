using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class StoreService : IStoreService
    {
        private readonly AppDbContext _context;
        public StoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Store>> GetAllAsync()
        {
            return await _context.Stores.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Store> GetByIdAsync(int? id)
        {
            return await _context.Stores.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }

}
