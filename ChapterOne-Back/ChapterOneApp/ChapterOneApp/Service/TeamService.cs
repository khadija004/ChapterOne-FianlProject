using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class TeamService : ITeamService
    {
        private readonly AppDbContext _context;
        public TeamService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Team>> GetAllAsync() => await _context.Teams
                                                                     .Include(t => t.Position)
                                                                     .ToListAsync();
        public async Task<Team> GetFullDataByIdAsync(int? id) => await _context.Teams
                                                                                .Include(t => t.Position)
                                                                                .FirstOrDefaultAsync(m => m.Id == id);
    }
}
