using System.Collections.Generic;
using EasyQMeetup.Domain.Events;

namespace EasyQMeetup.Domain
{
    public interface IDomainEvent {}
    public class Meetup
    {
        private readonly ICollection<IDomainEvent> _events;

        public Meetup(int id, string description)
        {
            _events = new List<IDomainEvent>();
        }

        public void RSVP(string userName, int plusNumber = 0) =>
            _events.Add(new RSVPConfirmedEvent(userName, plusNumber));

        public void CancelRSVP(string userName) =>
            _events.Add(new RSVPCancelledEvent(userName));
     
        public IEnumerable<IDomainEvent> GetEvents() => _events;
    }
}
