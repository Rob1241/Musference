namespace Musference.Models
{
    public class ReportedQuestion
    {
        public int Id { get; set; }
        public Question ReportQuestion { get; set; }
        public User UserThatReported { get; set; }
        public string reason { get; set; }
    }
}
