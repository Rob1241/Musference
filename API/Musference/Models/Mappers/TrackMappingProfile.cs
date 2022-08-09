using AutoMapper;
using Musference.Models.DTOs;

namespace Musference.Models.Mappers
{
    public class TrackMappingProfile : Profile
    {
        public TrackMappingProfile()
        {
            CreateMap<Track, GetTrackDto>();
            CreateMap<AddTrackDto, Track>();
        }
    }
}
