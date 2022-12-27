namespace Musference.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }
        public DateTime DateAdded { get; set; }
        public int Pluses { get; set; }
        //public int Minuses { get; set; }
        public User User { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Answer> Answers { get; set; }
        public int AnswersAmount { get; set; }
        public int Views { get; set; }
        public List<User> UsersThatLiked { get; set; }
        public IFormFile Audio { get; set; }
        public IFormFile Image { get; set; }
        public string AudioFile { get; set; }
        public string ImageFile { get; set; }

    }
}
