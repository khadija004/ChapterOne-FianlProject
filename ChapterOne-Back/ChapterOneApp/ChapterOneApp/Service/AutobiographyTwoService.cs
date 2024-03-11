using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class AutobiographyTwoService : IAutobiographyTwoService
    {
        private readonly AppDbContext _context;
        public AutobiographyTwoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AutobiographyTwo>> GetAllAsync()
        {
            return await _context.AutobiographyTwos.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<AutobiographyTwo> GetByIdAsync(int? id)
        {
            return await _context.AutobiographyTwos.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
