namespace Musference.Models.DTOs
{
    public class GetTrackDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Likes { get; set; }
        //public List<Tag> Tags { get; set; }
        public DateTime DateAdded { get; set; }
        public int TimesListened { get; set; }
        public int Length { get; set; }
    }
}
