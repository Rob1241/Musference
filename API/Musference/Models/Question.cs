namespace Musference.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }
        public DateTime DateAdded { get; set; }
        public int Pluses { get; set; }
        public int Minuses { get; set; }

        public int UserId { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Answer> Answers { get; set; }

        public int AnswersAmount { get; set; }

        public int Views { get; set; }

    }
}
