using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class WrapperService : IWrapperService
    {
        private readonly AppDbContext _context;
        public WrapperService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Wrapper>> GetAllAsync()
        {
            return await _context.Wrappers.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Wrapper> GetByIdAsync(int? id)
        {
            return await _context.Wrappers.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }

}
