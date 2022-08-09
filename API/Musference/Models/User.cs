namespace Musference.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public int Reputation { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public List<User> FollowedUsers { get; set; }
        public List<User> FollowingUsers { get; set; }
        public List<Track> Tracks { get; set; }
    }
}
