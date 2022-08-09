namespace Musference.Models
{
    public class AuthenticationModel
    {
        public string JwtKey { get; set; }
        public int JwtExpireHours { get; set; }
        public string JwtIssuer { get; set; }

    }
}
