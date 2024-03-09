using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;
        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authors.Include(m => m.ProductAuthors).Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Author> GetByIdAsync(int? id)
        {
            return await _context.Authors.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
