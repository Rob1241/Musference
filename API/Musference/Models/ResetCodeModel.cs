namespace Musference.Models
{
    public class ResetCodeModel
    {
        public int Id { get; set; }
        public string HashedCode { get; set; }
        public DateTime Expiration { get; set; }
        public int UserId { get; set; }
    }
}
