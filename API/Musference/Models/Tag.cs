namespace Musference.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Track> Tracks { get; set; }
        public List<Question> Questions { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
