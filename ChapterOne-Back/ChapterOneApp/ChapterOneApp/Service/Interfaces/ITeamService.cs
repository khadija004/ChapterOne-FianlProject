using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface ITeamService
    {
        Task<List<Team>> GetAllAsync();
        Task<Team> GetFullDataByIdAsync(int? id);
    }
}
