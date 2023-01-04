using AutoMapper;
using Musference.Models.DTOs;
using Musference.Models.Entities;

namespace Musference.Models.Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<User, GetUserDto>();
            CreateMap<User, GetAllUsersDto>();
        }
    }
}
