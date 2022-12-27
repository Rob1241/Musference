using Musference.Models.DTOs;

namespace Musference.Models.EndpointModels
{
    public class ReportedUsersResponse
    {
        public List<ReportedUser> ReportedUsers { get; set; } = new List<ReportedUser>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
