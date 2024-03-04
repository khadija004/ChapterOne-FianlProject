namespace ChapterOneApp.Models
{
    public class Blog : BaseEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public int CompilerId { get; set; }
        public Compiler Compiler { get; set; }
        public ICollection<BlogComment> BlogComments { get; set; }
    }
}
