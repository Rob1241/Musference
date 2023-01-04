using Musference.Models.DTOs;

namespace Musference.Models.EndpointModels.Question
{
    public class QuestionsResponse
    {
        public List<GetAllQuestionDto> Questions { get; set; } = new List<GetAllQuestionDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
