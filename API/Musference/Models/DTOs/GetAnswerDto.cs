namespace Musference.Models.DTOs
{
    public class GetAnswerDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
        public DateTime DateAdded { get; set; }
        public int Pluses { get; set; }
    }
}
