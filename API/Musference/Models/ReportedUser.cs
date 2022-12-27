namespace Musference.Models
{
    public class ReportedUser
    {
        public int Id { get; set; }
        public User ReporteddUser { get; set; }
        public User UserThatReported { get; set; }
        public string reason { get; set; }
    }
}
