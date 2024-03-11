using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class AutobiographyFourService : IAutobiographyFourService
    {
        private readonly AppDbContext _context;
        public AutobiographyFourService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AutobiographyFour>> GetAllAsync()
        {
            return await _context.AutobiographyFours.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<AutobiographyFour> GetByIdAsync(int? id)
        {
            return await _context.AutobiographyFours.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
