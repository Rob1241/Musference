namespace Musference.Models.DTOs
{
    public class GetTrackDto
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public User User { get; set; }
        public List<Tag> Tags { get; set; }
        public DateTime DateAdded { get; set; }
        public int TimesListened { get; set; }
        public int Length { get; set; }
    }
}
