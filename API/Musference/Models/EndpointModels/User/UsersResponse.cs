using Musference.Models.DTOs;

namespace Musference.Models.EndpointModels.User
{
    public class UsersResponse
    {
        public List<GetAllUsersDto> Users { get; set; } = new List<GetAllUsersDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
