using AutoMapper;
using Musference.Models.DTOs;

namespace Musference.Models.Mappers;


public class TagMappingProfile : Profile
{
    public TagMappingProfile()
    {
        CreateMap<Tag, GetTagDto>();
    }
}