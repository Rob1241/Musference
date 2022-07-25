using Microsoft.EntityFrameworkCore;
using Musference.Models;

namespace Musference.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) :base(options)
            {
            
            }

        public DbSet<Answer> AnswersDbSet { get; set; }
        public DbSet<Question> QuestionsDbSet { get; set; }

        public DbSet<Tag> TagsDbSet { get; set; }

        public DbSet<User> UsersDbSet { get; set; }
        

    }
    
}
