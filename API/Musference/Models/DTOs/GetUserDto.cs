namespace Musference.Models.DTOs
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Login { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public int Reputation { get; set; }

        public string Description { get; set; }
        public string Contact { get; set; }
        
        public string City { get; set; }

        public string Country { get; set; }
        public List<GetTrackDto> TracksDto { get; set; }
        public List<GetQuestionDto> QuestionsDto { get; set; }
    }
}
