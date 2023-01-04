using Musference.Data;
using Musference.Models.Entities;

namespace Musference
{
    public class Seeder
    {
        private readonly DataBaseContext _context;
        public Seeder(DataBaseContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if(_context.Database.CanConnect())
            {
                if(!_context.Roles.Any())
                {
                    var roles = GetRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };
            return roles;
        }
    }
}
