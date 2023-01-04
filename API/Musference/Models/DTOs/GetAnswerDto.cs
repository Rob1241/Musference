namespace Musference.Models.DTOs
{
    public class GetAnswerDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public int UserReputation { get; set; }
        public DateTime DateAdded { get; set; }
        public int Pluses { get; set; }
    }
}
