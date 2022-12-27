namespace Musference.Models.DTOs
{
    public class GetUserDto
    {
        public string Login { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public int Reputation { get; set; }

        public string Description { get; set; }
        public string Contact { get; set; }
        
        public string City { get; set; }

        public string Country { get; set; }
        public List<Track> Tracks { get; set; }
        public List<Question> Questions { get; set; }
    }
}
