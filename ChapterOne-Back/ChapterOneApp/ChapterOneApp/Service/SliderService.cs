using ChapterOneApp.Data;
using ChapterOneApp.Models;
using ChapterOneApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChapterOneApp.Service
{
    public class SliderService : ISliderService
    {
        readonly AppDbContext _context;
        public SliderService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Slider>> GetAll()
        {
            return await _context.Sliders.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Slider> GetById(int? id)
        {
            return await _context.Sliders.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
