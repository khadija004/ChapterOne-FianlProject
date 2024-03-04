namespace ChapterOneApp.Models
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ProductAuthor> ProductAuthors { get; set; }
    }
}
