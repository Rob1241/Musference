namespace Musference.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Tag> Tags { get; set; }
        public DateTime DateAdded { get; set; }
        public int  TimesListened { get; set; }
        public int Length { get; set; }
        public int Likes { get; set; }
    }
}
