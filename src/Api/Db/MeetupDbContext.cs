using EasyQMeetup.Domain;
using Microsoft.EntityFrameworkCore;

namespace Api.Db
{
    public class MeetupDbContext : DbContext
    {
        public DbSet<Meetup> Meetups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
            optionsBuilder
                .UseInMemoryDatabase();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meetup>().HasKey(c => c.Id);
        }
    }
}