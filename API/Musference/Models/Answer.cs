namespace Musference.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string QuestionId { get; set; }

        public int UserId { get; set; }
        public DateTime DateAdded { get; set; }
        public int Pluses { get; set; }
        public int Minuses { get; set; }
    }
}
