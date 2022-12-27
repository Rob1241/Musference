namespace Musference.Models.DTOs
{
    public class AddAnswerDto
    {
        public string Content { get; set; }
        public IFormFile Audio { get; set; }
        public IFormFile Image { get; set; }

    }
}
