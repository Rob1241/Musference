using Musference.Models.DTOs;

namespace Musference.Models.EndpointModels
{
    public class TagResponse
    {
        public List<GetTagDto> Tags { get; set; } = new List<GetTagDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
