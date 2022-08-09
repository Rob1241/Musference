using AutoMapper;
using Musference.Models.DTOs;

namespace Musference.Models.Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<User, GetUserDto>();
        }
    }
}
