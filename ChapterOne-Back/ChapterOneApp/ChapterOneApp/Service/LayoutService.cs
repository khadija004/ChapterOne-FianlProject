using ChapterOneApp.Data;
using ChapterOneApp.Service.Interfaces;

namespace ChapterOneApp.Service
{
    public class LayoutService : ILayoutService
    {
        private readonly AppDbContext _context;
        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, string> GetSettingsData()
        {
            Dictionary<string, string> settings = _context.Settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            return settings;
        }

        public Dictionary<string, string> GetHeaderBackgroundData() => _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
    }
}
