using AutoMapper;
using Musference.Models.DTOs;
using Musference.Models.Entities;

namespace Musference.Models.Mappers
{
    public class TrackMappingProfile : Profile
    {
        public TrackMappingProfile()
        {
            CreateMap<Track, GetTrackDto>()
                    .ForMember(t => t.Username, c => c.MapFrom(u => u.User.Name));
            CreateMap<AddTrackDto, Track>();
        }
    }
}
