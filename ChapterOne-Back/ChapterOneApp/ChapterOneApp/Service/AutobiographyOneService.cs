using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class AutobiographyOneService : IAutobiographyOneService
    {
        private readonly AppDbContext _context;
        public AutobiographyOneService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AutobiographyOne>> GetAllAsync()
        {
            return await _context.AutobiographyOnes.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<AutobiographyOne> GetByIdAsync(int? id)
        {
            return await _context.AutobiographyOnes.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
