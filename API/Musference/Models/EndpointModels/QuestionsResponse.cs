using Musference.Models.DTOs;

namespace Musference.Models.EndpointModels
{
    public class QuestionsResponse
    {
        public List<GetQuestionDto> Questions { get; set; } = new List<GetQuestionDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
