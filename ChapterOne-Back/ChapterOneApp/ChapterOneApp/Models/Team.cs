namespace ChapterOneApp.Models
{
    public class Team : BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
