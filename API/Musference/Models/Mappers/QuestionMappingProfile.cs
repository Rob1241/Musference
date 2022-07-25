using AutoMapper;
using Musference.Data;
using Musference.Models.DTOs;
using System.Linq;

namespace Musference.Models.Mappers;


public class QuestionMappingProfile :Profile
{
    public QuestionMappingProfile()
    {
        CreateMap<AddQuestionDTO, Question>();
        CreateMap<Question, GetQuestionDto>();
    }
}
