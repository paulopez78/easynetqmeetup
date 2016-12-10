using System;
using System.Collections.Generic;
using EasyQMeetup.Domain.Events;

namespace EasyQMeetup.Domain
{
    public class Meetup : IAggregate
    {
        private readonly ICollection<IDomainEvent> _events = new List<IDomainEvent>();
        
        public Guid Id { get; private set;}

        public string Description { get; private set;}

        //EF needed parameterless constructor
        public Meetup()
        {
        }

        public Meetup(string description)
        {
            Id = Guid.NewGuid();
            Description = description;
        }

        public void RSVP(string userName, int plusNumber = 0) =>
            _events.Add(new RSVPConfirmedEvent(userName, plusNumber));

        public void CancelRSVP(string userName) =>
            _events.Add(new RSVPCancelledEvent(userName));
     
        public IEnumerable<IDomainEvent> GetEvents() => _events;
    }
}
