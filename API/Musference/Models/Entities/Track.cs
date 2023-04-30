using System.ComponentModel.DataAnnotations.Schema;

namespace Musference.Models.Entities
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public List<User> UsersThatLiked { get; set; }
        public DateTime DateAdded { get; set; }
        public int Likes { get; set; }
        public string AudioFile { get; set; }
        public string LogoFile { get; set; }
    }
}
