using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class AutobiographyThreeService : IAutobiographyThreeService
    {
        private readonly AppDbContext _context;
        public AutobiographyThreeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AutobiographyThree>> GetAllAsync()
        {
            return await _context.AutobiographyThrees.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<AutobiographyThree> GetByIdAsync(int? id)
        {
            return await _context.AutobiographyThrees.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
