namespace Musference.Models.DTOs
{
    public class AddQuestionDTO
    {
        public string Heading { get; set; }
        public string Content { get; set; }
        public string? AudioFile { get; set; } 
        public string? ImageFile { get; set; }
    }
}
