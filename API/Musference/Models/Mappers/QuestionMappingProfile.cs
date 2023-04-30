using AutoMapper;
using Musference.Models.DTOs;
using Musference.Models.Entities;

namespace Musference.Models.Mappers;


public class QuestionMappingProfile : Profile
{
    public QuestionMappingProfile()
    {
        CreateMap<AddQuestionDTO, Question>();
        CreateMap<Question, GetQuestionDto>()
                .ForMember(q => q.Username, c => c.MapFrom(u => u.User.Name))
                .ForMember(q => q.UserReputation, c => c.MapFrom(u => u.User.Reputation))
                .ForMember(q => q.UserImage, c => c.MapFrom(u => u.User.ImageFile));
        CreateMap<Question, GetAllQuestionDto>()
                .ForMember(q => q.Username, c => c.MapFrom(u => u.User.Name));
        CreateMap<Answer, GetAnswerDto>()
                .ForMember(a => a.Username, c => c.MapFrom(u => u.User.Name))
                .ForMember(a => a.UserReputation, c => c.MapFrom(u => u.User.Reputation))
                .ForMember(a => a.UserImage, c => c.MapFrom(u => u.User.ImageFile));
        CreateMap<AddAnswerDto, Answer>();
    }
}
