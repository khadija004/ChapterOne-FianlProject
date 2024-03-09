using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class PositionService : IPositionService
    {
        private readonly AppDbContext _context;
        public PositionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Position>> GetAllAsync()
        {
            return await _context.Positions.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Position> GetByIdAsync(int? id)
        {
            return await _context.Positions.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
