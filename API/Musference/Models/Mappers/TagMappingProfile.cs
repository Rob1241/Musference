using AutoMapper;
using Musference.Models.DTOs;
using Musference.Models.Entities;

namespace Musference.Models.Mappers;


public class TagMappingProfile : Profile
{
    public TagMappingProfile()
    {
        CreateMap<Tag, GetTagDto>();
    }
}