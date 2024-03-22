using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class SocialService : ISocialService
    {
        private readonly AppDbContext _context;
        public SocialService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Social>> GetSocialsAsync()
        {
            return await _context.Socials.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Social> GetByIdAsync(int? id)
        {
            return await _context.Socials.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
