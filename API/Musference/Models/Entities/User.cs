using System.ComponentModel.DataAnnotations.Schema;

namespace Musference.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public int Reputation { get; set; }
        public string Description { get; set; }

        public string Contact { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime DateAdded { get; set; }
        public List<Question> QuestionsLiked { get; set; }
        public List<Answer> AnswersLiked { get; set; }
        public List<Track> TracksLiked { get; set; }
        public List<Track> Tracks { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
        public string ImageFile { get; set; }
    }
}
