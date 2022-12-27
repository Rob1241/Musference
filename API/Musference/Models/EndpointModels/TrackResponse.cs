using Musference.Models.DTOs;

namespace Musference.Models.EndpointModels
{
    public class TrackResponse
    {
        public List<GetTrackDto> Tracks { get; set; } = new List<GetTrackDto>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
