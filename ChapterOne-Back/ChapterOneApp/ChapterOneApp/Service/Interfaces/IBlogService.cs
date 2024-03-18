using ChapterOneApp.Models;

namespace ChapterOneApp.Service.Interfaces
{
    public interface IBlogService
    {
        Task<List<Blog>> GetBlogs();
        Task<List<Blog>> GetPaginateDatas(int page, int take);
        Task<int> GetCountAsync();
        Task<Blog> GetById(int? id);
        Task<IEnumerable<Compiler>> GetCompilersAsync();
        Task<List<BlogComment>> GetComments();
        Task<BlogComment> GetCommentByIdWithBlog(int? id);
        Task<BlogComment> GetCommentById(int? id);
    }
}
