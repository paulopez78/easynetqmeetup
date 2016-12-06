using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Db;
using EasyNetQ;
using EasyQMeetup.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class MeetupController
    {
        private readonly MeetupDbContext _dbContext;
        private readonly IBus _bus;

        public MeetupController(MeetupDbContext dbContext)
        {
            _dbContext = dbContext;
            _bus = RabbitHutch.CreateBus("amqp://guest:guest@localhost:5672");
        }

        [HttpGet]
        public async Task<IEnumerable<Meetup>> Get() =>  
            await _dbContext.Meetups.ToListAsync();

        [HttpGet("{id}")]
        public async Task<Meetup> Get(Guid id) => 
            await _dbContext.Meetups.FirstOrDefaultAsync(x => x.Id == id);

        [HttpPost]
        public async Task<Meetup> CreateMeetup([FromBody]string description)
        {
            var meetup = new Meetup(description);
            _dbContext.Meetups.Add(meetup);
            await _dbContext.SaveChangesAsync();
            return meetup;
        }

        [HttpPost]
        [Route("api/[controller]/{id}/rsvp")]
        public async Task AddRSVP(Guid id, [FromBody]RSVPModel rsvp)
        {
            var meetup = _dbContext.Meetups.FirstOrDefault(x => x.Id == id);
            meetup.RSVP(rsvp.UserName, rsvp.PlusNumber);

            await _dbContext.SaveChangesAsync();
            await PublishEvents(meetup.GetEvents());
        }

        // [HttpDelete]
        // [Route("api/[controller]/{id}/rsvp")]
        // public async Task CancelRSVP(Guid id, [FromBody]string name)
        // {
        //     var meetup = _dbContext.Meetups.FirstOrDefault(x => x.Id == id);
        //     meetup.CancelRSVP(name);

        //     await _dbContext.SaveChangesAsync();
        //     await PublishEvents(meetup.GetEvents());
        // }

        private async Task PublishEvents(IEnumerable<IDomainEvent> events)
        {
            foreach (dynamic domainEvent in events)
                await _bus.PublishAsync(domainEvent);
        }
    }
}
