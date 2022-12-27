using AutoMapper;
using Musference.Models.DTOs;

namespace Musference.Models.Mappers;


public class QuestionMappingProfile : Profile
{
    public QuestionMappingProfile()
    {
        CreateMap<AddQuestionDTO, Question>();
        CreateMap<Question, GetQuestionDto>();
        CreateMap<Answer, GetAnswerDto>();
        CreateMap<AddAnswerDto, Answer>();
    }
}
