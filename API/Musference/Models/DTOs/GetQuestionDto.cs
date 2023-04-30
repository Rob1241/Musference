namespace Musference.Models.DTOs
{
    public class GetQuestionDto
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }
        public DateTime DateAdded { get; set; }
        public int Pluses { get; set; }
        public int Minuses { get; set; }

        public string UserImage { get; set; }

        public string Username { get; set; }
        public int UserId { get; set; }
        public int UserReputation { get; set; }
        public int AnswersAmount { get; set; }

        public int Views { get; set; }
        public string AudioFile { get; set; }
        public string ImageFile { get; set; }
    }
}
