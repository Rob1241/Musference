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

        public DbSet<Role> Roles { get; set; }
        public DbSet<ResetCodeModel> PasswordResetDbSet { get; set; }
        public DbSet<ReportedQuestion> ReportedQuestionsDbSet { get; set; }
        public DbSet<ReportedTrack> ReportedTracksDbSet { get; set; }
        public DbSet<ReportedAnswer> ReportedAnswersDbSet { get; set; }

        public DbSet<ReportedUser> ReportedUsersDbSet { get; set; }

        public DbSet<Track> TracksDbSet { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Country).IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(u => u.Description).IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(u => u.City).IsRequired(false);

        }

    }
    
}
