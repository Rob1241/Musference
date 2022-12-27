namespace Musference.Models
{
    public class ReportedAnswer
    {
        public int Id { get; set; }
        public Answer ReportAnswer { get; set; }
        public User UserThatReported { get; set; }
        public string reason { get; set; }
    }
}
