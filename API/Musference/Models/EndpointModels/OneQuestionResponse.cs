using Musference.Models.DTOs;

namespace Musference.Models.EndpointModels
{
    public class OneQuestionResponse
    {
        public List<GetAnswerDto> Answers { get; set; } = new List<GetAnswerDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public GetQuestionDto Question { get; set; }
    }
}
