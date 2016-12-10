using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Db;
using EasyQMeetup.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class MeetupController
    {
        private readonly MeetupDbContext _dbContext;

        public MeetupController(MeetupDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Meetup>> Get() =>  
            await _dbContext.Meetups.ToListAsync();

        [HttpGet("{id}")]
        public async Task<Meetup> Get(Guid id) => 
            await _dbContext.Meetups.FirstOrDefaultAsync(x => x.Id == id);

        [HttpPost]
        public async Task<Meetup> CreateMeetup([FromBody]MeetupModel model)
        {
            var meetup = new Meetup(model.Description);
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
        }

        [HttpDelete]
        [Route("api/[controller]/{id}/rsvp")]
        public async Task CancelRSVP(Guid id, [FromBody]RSVPModel rsvp)
        {
            var meetup = _dbContext.Meetups.FirstOrDefault(x => x.Id == id);
            meetup.CancelRSVP(rsvp.UserName);
            await _dbContext.SaveChangesAsync();
        }
    }
}
