namespace Musference.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Question Question { get; set; }

        public User User { get; set; }
        public DateTime DateAdded { get; set; }
        public int Pluses { get; set; }
        public int Minuses { get; set; }
    }
}
