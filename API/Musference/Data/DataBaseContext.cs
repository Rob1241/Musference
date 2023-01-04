using Microsoft.EntityFrameworkCore;
using Musference.Models.Entities;

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
        //public DbSet<ResetCodeModel> PasswordResetDbSet { get; set; }
        //public DbSet<ReportedQuestion> ReportedQuestionsDbSet { get; set; }
        //public DbSet<ReportedTrack> ReportedTracksDbSet { get; set; }
        //public DbSet<ReportedAnswer> ReportedAnswersDbSet { get; set; }

        //public DbSet<ReportedUser> ReportedUsersDbSet { get; set; }

        public DbSet<Track> TracksDbSet { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Country).IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(u => u.Description).IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(u => u.City).IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(u => u.Contact).IsRequired(false);
            modelBuilder.Entity<Answer>()
                        .HasOne(a => a.User)
                        .WithMany(u => u.Answers);
            modelBuilder.Entity<Answer>()
                        .HasOne(a => a.Question)
                        .WithMany(q => q.Answers);
            modelBuilder.Entity<Question>()
                        .HasOne(q => q.User)
                        .WithMany(u => u.Questions);
            modelBuilder.Entity<Track>()
                        .HasOne(t => t.User)
                        .WithMany(u => u.Tracks);
        }

    }
    
}
