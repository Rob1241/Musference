using Musference.Models.Entities;

namespace Musference.Models
{
    public class ReportedTrack
    {
        public int Id { get; set; }
        public Track ReportTrack { get; set; }
        public User UserThatReported { get; set; }
        public string reason { get; set; }
    }
}
