using System.Collections.Generic;

namespace EasyQMeetup.Domain
{
    public interface IAggregate 
    {
        IEnumerable<IDomainEvent> GetEvents();
    }
}
