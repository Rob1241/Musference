using Musference.Models.DTOs;

namespace Musference.Models.EndpointModels
{
    public class UsersResponse
    {
        public List<GetUserDto> Questions { get; set; } = new List<GetUserDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
