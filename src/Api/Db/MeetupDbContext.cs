using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyQMeetup.Domain;
using Microsoft.EntityFrameworkCore;

namespace Api.Db
{
    public class MeetupDbContext : DbContext
    {
        private readonly IBus _bus;

        public MeetupDbContext()
        {
            _bus = RabbitHutch.CreateBus("amqp://guest:guest@localhost:5672");
        }

        public DbSet<Meetup> Meetups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
            optionsBuilder.UseInMemoryDatabase();

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.Entity<Meetup>().HasKey(c => c.Id);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken() )
        {
            var result = await base.SaveChangesAsync( cancellationToken );
            await PublishEvents();
            return result;
        }
        
        private async Task PublishEvents()
        {
            foreach (var entityEntry in ChangeTracker.Entries())
            {
                var entity = entityEntry.Entity as IAggregate;
                if (entity != null)
                {
                    foreach(dynamic domainEvent in entity.GetEvents())
                        await _bus.PublishAsync(domainEvent);
                }
            }
        }
    }
}